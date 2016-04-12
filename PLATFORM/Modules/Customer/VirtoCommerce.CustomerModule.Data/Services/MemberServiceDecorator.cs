using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    /// <summary>
    /// Used for decorate multiple members services (members extension point)
    /// Translate all IMemberServices calls to IMemberServices depend on configuration 
    /// Also represent Members types factory
    /// Usage:
    /// </summary>
    /// <example>
    /// To register new member type:
    ///   MemberServiceDecorator.RegisterMemberTypes(typeof(Contact))
    ///                              .WithService(contactService)
    ///                              .WithSearchService(contactSearchService);
    ///  To override already exist member type:
    ///    MemberServiceDecorator.OverrideMemberType<Contact>()
    ///                              .WithType<NewContact>()
    ///                              .WithService(newContactService)
    ///                              .WithSearchService(newContactSearchService);
    ///  To override MembersSearchCriteria type to other derived type:
    ///  MemberServiceDecorator.UseMemberSearchCriteriaType<NewmemberSearchCriteria>();
    /// </example>
    public sealed class MemberServiceDecorator : IMemberService, IMemberFactory, IMemberSearchService
    {
        private List<MemberTypeMappingConfig> _memberMappings = new List<MemberTypeMappingConfig>();
        
        private Type _memberSearchCriteriaType = typeof(MembersSearchCriteria);

        /// <summary>
        /// Override MembersSearchCriteria to other (used in API controllers to construct criteria from input JSON)
        /// </summary>
        /// <typeparam name="T">MembersSearchCriteria derived type</typeparam>
        /// <returns></returns>
        public MemberServiceDecorator UseMemberSearchCriteriaType<T>() where T : MembersSearchCriteria
        {
            _memberSearchCriteriaType = typeof(T);
            return this;
        }

        /// <summary>
        /// Register new member type (fluent method)
        /// </summary>
        /// <param name="memberTypes">new member types</param>
        /// <returns>MemberTypeMappingConfig instance to continue configuration through fluent syntax</returns>
        public MemberTypeMappingConfig RegisterMemberTypes(params Type[] memberTypes)
        {
            var intersection = _memberMappings.SelectMany(x => x.AllSupportedMemberTypes).Intersect(memberTypes);
            if (intersection.Any())
            {
                throw new ArgumentException("Types already registered " + string.Join(", ", intersection.Select(x=>x.Name)));
            }
            var notMemberTypes = memberTypes.Where(x => !x.IsDerivativeOf(typeof(Member)));
            if(notMemberTypes.Any())
            {
                throw new ArgumentException(string.Join(", ", notMemberTypes.Select(x => x.Name)), " should be inherited from Member type");
            }
            var retVal = new MemberTypeMappingConfig(memberTypes);
            _memberMappings.Add(retVal);
            return retVal;
        }

        /// <summary>
        /// Override already registered member type to new 
        /// </summary>
        /// <typeparam name="T">overridden  member type</typeparam>
        /// <returns>MemberTypeMappingConfig instance to continue configuration through fluent syntax</returns>
        public MemberTypeMappingConfig OverrideMemberType<T>() where T : Member
        {
            var memberType = typeof(T);           
            var existMapping = _memberMappings.FirstOrDefault(x => x.MemberTypes.Contains(memberType));
            if(existMapping == null)
            {
                throw new ArgumentException("Not found");
            }
            existMapping.MemberTypes.Remove(memberType);

            var newMemberMapping = new MemberTypeMappingConfig()
            {
                MemberService = existMapping.MemberService,
                MemberSearchService = existMapping.MemberSearchService
            };
            _memberMappings.Add(newMemberMapping);
            return newMemberMapping;
        }

      
        #region IMembersFactory Members
        /// <summary>
        /// Factory method created concrete Member type instance base on member type name
        /// </summary>
        /// <param name="memberTypeName"></param>
        /// <returns></returns>
        public Member TryCreateMember(string memberTypeName)
        {
            Member retVal = null;
            var knownMemberType = _memberMappings.Select(x => x.ResolveMemberType(memberTypeName)).FirstOrDefault(x => x != null);
            if (knownMemberType != null)
            {
                retVal = Activator.CreateInstance(knownMemberType) as Member;
            }
            return retVal;
        }

        public MembersSearchCriteria CreateMemberSearchCriteria()
        {
            return Activator.CreateInstance(_memberSearchCriteriaType) as MembersSearchCriteria;
        }
        #endregion

        #region IMemberService Members

        public Member[] GetByIds(string[] memberIds, string[] memberTypes = null)
        {
            var retVal = new ConcurrentBag<Member>();
            Parallel.ForEach(_memberMappings, (x) =>
            {
                foreach (var member in x.MemberService.GetByIds(memberIds, x.AllSupportedMemberTypeNames.ToArray()))
                {
                    retVal.Add(member);
                }
            });
            return retVal.ToArray();
        }

        public void CreateOrUpdate(Member[] members)
        {
            Parallel.ForEach(_memberMappings, (memberMapping) =>
            {
                var matchMembers = members.Where(x => memberMapping.AllSupportedMemberTypeNames.Contains(x.MemberType, StringComparer.OrdinalIgnoreCase)).ToArray();
                if(!matchMembers.IsNullOrEmpty())
                {
                    memberMapping.MemberService.CreateOrUpdate(matchMembers);
                }              
            });
        }

        public void Delete(string[] ids, string[] memberTypes = null)
        {
            var retVal = new ConcurrentBag<Member>();
            Parallel.ForEach(_memberMappings, (x) =>
            {
                x.MemberService.Delete(ids, x.AllSupportedMemberTypeNames.ToArray());

            });
        }
        #endregion

        #region IMemberSearchService Members
        /// <summary>
        /// Search in multiple data sources. 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public MembersSearchResult SearchMembers(MembersSearchCriteria criteria)
        {
            var retVal = new MembersSearchResult();
            var skip = criteria.Skip;
            var take = criteria.Take;
            var memberTypes = criteria.MemberTypes;
            /// !!!Ahtung!!!: Because members can be searched in multiple data sources we have to always use sorting by memberType field (asc or desc) 
            /// instead pagination will not works properly
            var sortByMemberType = criteria.SortInfos.FirstOrDefault(x => string.Equals(x.SortColumn, "memberType", StringComparison.OrdinalIgnoreCase)) ?? new SortInfo { SortColumn = "memberType" };
            var sortInfos = criteria.SortInfos.Where(x => x != sortByMemberType);
            criteria.Sort = SortInfo.ToString(new[] { sortByMemberType }.Concat(sortInfos));
           
            foreach (var memberMapping in _memberMappings)
            {            
                criteria.MemberTypes = memberTypes.IsNullOrEmpty() ? memberMapping.AllSupportedMemberTypeNames.ToArray() : memberMapping.AllSupportedMemberTypeNames.Intersect(memberTypes, StringComparer.OrdinalIgnoreCase).ToArray();
                if (!criteria.MemberTypes.IsNullOrEmpty() && criteria.Take >= 0)
                {
                    var result = memberMapping.MemberSearchService.SearchMembers(criteria);
                    retVal.Members.AddRange(result.Members);
                    retVal.TotalCount += result.TotalCount;
                    criteria.Skip = Math.Max(0, skip - retVal.TotalCount);
                    criteria.Take = Math.Max(0, take - result.Members.Count());
                }
            }
            //restore back criteria property values
            criteria.Skip = skip;
            criteria.Take = take;
            criteria.MemberTypes = memberTypes;
            return retVal;
        } 
        #endregion
    }

    /// <summary>
    /// Helper class contains member type mapping information
    /// </summary>
    public sealed class MemberTypeMappingConfig
    {
        public MemberTypeMappingConfig(Type[] memberTypes = null)
        {
            MemberTypes = new List<Type>();
            if(memberTypes != null)
            {
                MemberTypes.AddRange(memberTypes);
            }
        }
        /// <summary>
        /// Supported member types
        /// </summary>
        public ICollection<Type> MemberTypes { get; private set; }
        /// <summary>
        /// CRUD service for supported member types
        /// </summary>
        public IMemberService MemberService { get; set; }
        /// <summary>
        /// Search service for supported member types
        /// </summary>
        public IMemberSearchService MemberSearchService { get; set; }

        public MemberTypeMappingConfig WithService(IMemberService memberService)
        {
            MemberService = memberService;
            return this;
        }
        public MemberTypeMappingConfig WithSearchService(IMemberSearchService memberSearchService)
        {
            MemberSearchService = memberSearchService;
            return this;
        }

        public MemberTypeMappingConfig WithType<T>() where T : Member
        {
            var memberType = typeof(T);
            if(MemberTypes.Contains(memberType))
            {
                throw new ArgumentException("Already exist");
            }
            MemberTypes.Add(memberType);
            return this;                   
        }

        public Type ResolveMemberType(string memberTypeName)
        {
            return MemberTypes.FirstOrDefault(x => x.GetTypeInheritanceChainTo(typeof(Member)).Any(t=> string.Equals(memberTypeName, t.Name, StringComparison.OrdinalIgnoreCase)));
        }

        public IEnumerable<string> AllSupportedMemberTypeNames
        {
            get
            {
                return AllSupportedMemberTypes.Select(x => x.Name);
            }
        }

        public IEnumerable<Type> AllSupportedMemberTypes
        {
            get
            {
                return MemberTypes.SelectMany(x => x.GetTypeInheritanceChainTo(typeof(Member))).Distinct().ToArray();
            }
        }
    }



}
