using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Reporting.Model
{
    public class ReportFolder
    {
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public IEnumerable<ReportFolder> SubFoldersList { get; set; }
    }
}
