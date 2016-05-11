using System;
using System.Data.Entity.Infrastructure;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    /// <summary>
    /// An <see cref="EntityPrimaryKeyGeneratorInterceptor">PrimaryKeyGenerator</see> that generates <see cref="System.Guid"/> values 
    /// using a strategy suggested by Jimmy Nilsson's 
    /// <a href="http://www.informit.com/articles/article.asp?p=25862">article</a>
    /// on <a href="http://www.informit.com">informit.com</a>. 
    /// Copied from <a href="https://github.com/nhibernate/nhibernate-core/blob/master/src/NHibernate/Id/GuidCombGenerator.cs">NHibernate</a>
    /// </summary>
    /// <remarks>
    /// <p>
    ///	Example 1
    ///	You can configure at repository level to use this primary key generator. 
    ///	This will allow you to cherry pick and choose different primary key generator for each repository.
    ///	<code>new CatalogRepositoryImpl(_connectionStringName, new GuidCombiPrimaryKeyGeneratorInterceptor())</code>
    /// </p>
    /// <p>
    ///	Example 2
    ///	If you want to use this primary key generator for all primary keys than you can update <see cref="EntityPrimaryKeyGeneratorInterceptor.OnBeforeInsert(DbEntityEntry, Entity)"/>
    ///	to avoid replacing all references.
    ///	<code>
    ///	Comment line -  entity.Id = Guid.NewGuid().ToString("N");
    ///	Add new line - 	entity.Id = GuidCombiPrimaryKeyGeneratorInterceptor.GenerateComb().ToString("N");
    ///	</code>
    /// </p>
    /// <p>
    /// The <c>comb</c> algorithm is designed to make the use of GUIDs as Primary Keys, Foreign Keys, 
    /// and Indexes nearly as efficient as ints.
    /// </p>
    /// <p>
    /// This code was contributed by Donald Mull in NHibernate project.
    /// </p>
    /// </remarks>
    public class GuidCombiPrimaryKeyGeneratorInterceptor : ChangeInterceptor<Entity>
    {
        private static readonly long BaseDateTicks = new DateTime(1900, 1, 1).Ticks;

        public override void OnBeforeInsert(DbEntityEntry entry, Entity entity)
        {
            base.OnBeforeInsert(entry, entity);

            if (entity.IsTransient())
            {
                entity.Id = GenerateComb().ToString("N");
            }
        }

        /// <summary>
        /// Generate a new <see cref="Guid"/> using the comb algorithm.
        /// </summary>
        public static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime now = DateTime.UtcNow;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - BaseDateTicks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
    }
}
