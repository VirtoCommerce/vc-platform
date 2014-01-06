using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Reporting.Helpers;
using VirtoCommerce.Foundation.Reporting.Model;

namespace VirtoCommerce.Foundation.Reporting.Services
{
    [UnityInstanceProviderServiceBehaviorAttribute]
    public class ReportingService : IReportingService
    {
        public const string RootFolder = "Reports";

        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IAssetRepository _assetRepository;

        public ReportingService(IAssetRepository assetRepository, IBlobStorageProvider blobStorageProvider)
        {
            _blobStorageProvider = blobStorageProvider;
            _assetRepository = assetRepository;
        }

        public IEnumerable<ReportInfo> GetReportsList(string folder = RootFolder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = RootFolder;
            }
            List<ReportInfo> list = new List<ReportInfo>();
            var folderObj = _assetRepository.GetFolderById(folder);
            GetReportItems(list, folderObj);

            return list;
        }

        public IEnumerable<ReportFolder> GetReportsFolders()
        {
            List<ReportFolder> foldersList = new List<ReportFolder>();
            var folder = _assetRepository.GetFolderById(RootFolder);
            var root = new ReportFolder
            {
                FolderName = folder.Name,
                FullPath = folder.FolderId,
                SubFoldersList = new List<ReportFolder>()
            }; 
            foldersList.Add(root);
            GetFolders(root.SubFoldersList as List<ReportFolder>, folder);

            return foldersList;
        }

        private void GetFolders(List<ReportFolder> list, Folder folder)
        {
            list.AddRange(
                _assetRepository.GetChildrenFolders(folder.FolderId)
                    .Select(delegate(Folder f)
                    {
                        var subFolder = new ReportFolder
                        {
                            FolderName = f.Name,
                            FullPath = f.FolderId,
                            SubFoldersList = new List<ReportFolder>()
                        };
                        GetFolders(subFolder.SubFoldersList as List<ReportFolder>, f);
                        return subFolder;
                    })
                    .ToList()
                );
        }

        private void GetReportItems(List<ReportInfo> list, Folder folder)
        {
            list.AddRange( _assetRepository.GetChildrenFolderItems(folder.FolderId)
                //.Where(w=>Path.GetExtension(w.Name)==".rdl")
                .Select(f=>new ReportInfo
                {
                    Name = Path.GetFileNameWithoutExtension(f.Name),
                    AssetPath = f.FolderItemId
                }).ToList() );

            foreach (var childFolder in _assetRepository.GetChildrenFolders(folder.FolderId))
            {
                GetReportItems(list, childFolder);
            }
        }


        public Stream GetReportFile(string reportFileName)
        {
            return _blobStorageProvider.OpenReadOnly(reportFileName);
        }

        public DataSet GetReportData(string reportFileName)
        {
            DataSet data = new DataSet();
            var report = new RdlFile(GetReportFile(reportFileName));
            foreach(var dataSet in report.DataSets)
            {
                var connectionString = ConnectionHelper.GetConnectionString(dataSet.DataSource.Name);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = dataSet.DataSource.ConnectionString;
                }

                using (var dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var table = ExecuteSQL(dbConnection, dataSet.CommandText);
                    table.TableName = dataSet.Name;
                    data.Tables.Add(table);
                }
            }

            return data;
        }

        private DataTable ExecuteSQL(SqlConnection dbConn, string sqlCommand, params object[] args)
        {
            using (var cmd = dbConn.CreateCommand())
            {
                cmd.CommandText = string.Format(sqlCommand, args);
                var adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
               
                return table;
            }
        }
    }
}
