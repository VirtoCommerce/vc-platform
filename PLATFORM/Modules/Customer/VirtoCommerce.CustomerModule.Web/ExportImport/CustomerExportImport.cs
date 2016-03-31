using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CustomerModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public Member[] Members { get; set; }
    }

    public sealed class CustomerExportImport
    {
        private readonly IMemberService _memberService;
     
        public CustomerExportImport(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
			var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback);

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} members importing...", backupObject.Members.Count());
            progressCallback(progressInfo);
            _memberService.CreateOrUpdate(backupObject.Members.OrderByDescending(x => x.MemberType).ToArray());

        }

            #region BackupObject

		public BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = "loading members...";
			progressCallback(progressInfo);

            var members = _memberService.SearchMembers(new SearchCriteria { DeepSearch = true, Take = int.MaxValue }).Members;

            var result = new BackupObject();
            result.Members = _memberService.GetByIds(members.Select(x => x.Id).ToArray()).OrderByDescending(x=> x.MemberType).ToArray();

            return result;
        }

        #endregion

    }
}