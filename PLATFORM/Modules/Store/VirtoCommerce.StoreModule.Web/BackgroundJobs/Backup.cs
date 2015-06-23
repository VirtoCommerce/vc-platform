using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.StoreModule.Web.BackgroundJobs
{
    public class Backup
    {
        #region Classes

        abstract class BackupBaseEntry
        {

            private string[] _ignoreProperties;
            public string[] IgnoreProperties
            {
                get { return _ignoreProperties ?? (_ignoreProperties = new string[0]); }
                set { _ignoreProperties = value ?? new string[0]; }
            }

            public string EntryName { get; set; }

            public abstract Type BackupObjectType { get; }
        }

        class BackupEntry : BackupBaseEntry
        {
            public object BackupObject { get; set; }

            public override Type BackupObjectType
            {
                get { return BackupObject.GetType(); }
            }

        }

        class ExtractEntry : BackupBaseEntry
        {
            public string BackupObjectTypeName { get; set; }

            public override Type BackupObjectType
            {
                get { return Type.GetType(BackupObjectTypeName); }
            }
        }

        #endregion

        #region Class fields

        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private ZipArchive _zipArchive;
        private List<ExtractEntry> _extpactMap = new List<ExtractEntry>();
        private readonly List<BackupEntry> _backupEntries = new List<BackupEntry>();
        private const string _mapEntryName = "map.xml";

        #endregion

        #region Constructor

        public Backup(IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver)
        {
            _blobStorageProvider = blobStorageProvider;
            _blobUrlResolver = blobUrlResolver;
        }

        #endregion

        #region To backup

        /// <summary>
        /// Add object to backup
        /// </summary>
        /// <param name="entryName">object backup name</param>
        /// <param name="backupObject"></param>
        /// <param name="ignoreList">Object property list to ignore for serialize process.</param>
        public void AddEntry(string entryName, object backupObject, string[] ignoreList = null)
        {
            if (backupObject == null)
            {
                throw new Exception("backupObject argument is null.");
            }
            if (string.IsNullOrEmpty(entryName))
            {
                throw new Exception("entryName argument is null or empty.");
            }
            _backupEntries.Add(new BackupEntry { EntryName = entryName, IgnoreProperties = ignoreList, BackupObject = backupObject });
        }

        /// <summary>
        /// Store backup.
        /// </summary>
        /// <param name="backupName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string Save(string backupName, string folderName = "temp")
        {
            if (string.IsNullOrEmpty(backupName))
            {
                throw new Exception("backupName argument is null or empty.");
            }

            if (!_backupEntries.Any())
            {
                throw new Exception("Empty buckup.");
            }

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var zipStream = new ZipOutputStream(outputMemoryStream))
                {
                    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                    zipStream.IsStreamOwner = false;
                    _extpactMap.Clear();

                    foreach (var entry in _backupEntries)
                    {
                        AddToBackup(entry, zipStream);
                    }
                    StoreBackupMap(zipStream);
                }

                outputMemoryStream.Position = 0;
                var uploadInfo = new UploadStreamInfo
                {
                    FileName = string.Format("{0}.zip", backupName),
                    FileByteStream = outputMemoryStream,
                    FolderName = folderName
                };

                var blobKey = _blobStorageProvider.Upload(uploadInfo);

                _backupEntries.Clear();

                //Return file url
                return _blobUrlResolver.GetAbsoluteUrl(blobKey);
            }
        }

        #endregion

        #region From backup

        /// <summary>
        /// Open zip file before extract
        /// </summary>
        /// <param name="fileUrl"></param>
        public void OpenBackup(string fileUrl)
        {
            _zipArchive = new ZipArchive(_blobStorageProvider.OpenReadOnly(fileUrl), ZipArchiveMode.Read, true);
            LoadBackupMap();
        }

        /// <summary>
        /// Close zip file after extact
        /// </summary>
        public void CloseBackup()
        {
            _zipArchive.Dispose();
            _extpactMap.Clear();
        }

        /// <summary>
        /// Serialize xml file to object by given file name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public T LoadObject<T>(string entryName) where T : class
        {
            ExtractEntry extractEntry = null;

            if (_extpactMap != null)
            {
                extractEntry = _extpactMap.FirstOrDefault(x => x.EntryName == entryName);
            }

            extractEntry = extractEntry
                ?? new ExtractEntry
                {
                    EntryName = entryName,
                    BackupObjectTypeName = typeof(T).AssemblyQualifiedName
                };

            return GetFromBackup<T>(extractEntry);
        }

        /// <summary>
        ///  Serialize xml files to objects by given file name mask 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entryMask"></param>
        /// <returns></returns>
        public IEnumerable<T> LoadObjectsByMask<T>(string entryMask) where T : class
        {
            return _extpactMap
                .Where(x => x.EntryName.StartsWith(entryMask, StringComparison.InvariantCultureIgnoreCase))
                .Select(o => LoadObject<T>(o.EntryName))
                .ToList();
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Serialize object and add one to backup
        /// </summary>
        /// <param name="backupEntry"></param>
        /// <param name="zipStream"></param>
        private void AddToBackup(BackupEntry backupEntry, ZipOutputStream zipStream)
        {
            // Serialize BackupEntry
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream) { AutoFlush = true })
                {
                    var type = backupEntry.BackupObject.GetType();

                    // Prepare ignored properties 
                    var xmlOver = GetIgnoreProperties(backupEntry);
                    var serializer = new XmlSerializer(type, xmlOver);
                    serializer.Serialize(streamWriter, backupEntry.BackupObject);

                    //Add result to zip
                    memoryStream.Position = 0;
                    var newEntry = new ZipEntry(backupEntry.EntryName) { DateTime = DateTime.Now };
                    zipStream.PutNextEntry(newEntry);
                    StreamUtils.Copy(memoryStream, zipStream, new byte[4096]);
                    zipStream.CloseEntry();
                    _extpactMap.Add(new ExtractEntry { BackupObjectTypeName = type.AssemblyQualifiedName, IgnoreProperties = backupEntry.IgnoreProperties, EntryName = backupEntry.EntryName });
                }
            }
        }

        /// <summary>
        /// Get object from backup and deserialize one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectDefinition"></param>
        /// <returns></returns>
        private T GetFromBackup<T>(ExtractEntry objectDefinition) where T : class
        {
            var entry = _zipArchive.Entries.FirstOrDefault(x => x.Name.Equals(objectDefinition.EntryName, StringComparison.InvariantCultureIgnoreCase));

            using (var reader = entry.Open())
            {
                var type = objectDefinition.BackupObjectType;

                // Prepare ignored properties 
                var xmlOver = GetIgnoreProperties(objectDefinition);
                var serializer = new XmlSerializer(type, xmlOver);
                var serializedObject = serializer.Deserialize(reader);

                return serializedObject as T;
            }
        }

        /// <summary>
        /// Store backup map
        /// BackupMap conteins all stored object name and their types of the backup.
        /// </summary>
        /// <param name="zipStream"></param>
        private void StoreBackupMap(ZipOutputStream zipStream)
        {
            AddToBackup(new BackupEntry { EntryName = _mapEntryName, BackupObject = _extpactMap.ToArray() }, zipStream);
        }
 
        /// <summary>
        /// Load backup map
        /// BackupMap conteins all stored object name and their types of the backup.
        /// </summary>
        private void LoadBackupMap()
        {
            var objectDefinition = new ExtractEntry
            {
                EntryName = _mapEntryName,
                BackupObjectTypeName = typeof(ExtractEntry[]).AssemblyQualifiedName
            };
            _extpactMap = GetFromBackup<ExtractEntry[]>(objectDefinition).ToList();
        }

        /// <summary>
        /// Complete fields list to ignore for serialize process.
        /// Extend given Ignore list with collection properties.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private XmlAttributeOverrides GetIgnoreProperties(BackupBaseEntry entry)
        {
            var xmlOver = new XmlAttributeOverrides();
            var xmlAttr = new XmlAttributes { XmlIgnore = true };

            var newIgnorePropertyNames = new List<string>(entry.IgnoreProperties);

            var properties = entry.BackupObjectType.GetProperties();

            // Create XmlAttributeOverrides by given IgnoreProperties and ICollection properties
            foreach (var propertyInfo in properties)
            {
                try
                {
                    if (propertyInfo.DeclaringType != null &&
                        (propertyInfo.PropertyType.Name.StartsWith("ICollection") || entry.IgnoreProperties.Any(x => x == propertyInfo.PropertyType.Name)))
                    {
                        newIgnorePropertyNames.Add(propertyInfo.Name);
                        xmlOver.Add(propertyInfo.DeclaringType, propertyInfo.Name, xmlAttr);
                    }
                }
                catch (Exception)
                {
                }
            }

            // Update IgnoreProperties
            entry.IgnoreProperties = newIgnorePropertyNames.ToArray();
            return xmlOver;
        }

        #endregion

    }
}