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

        public abstract class BackupBaseEntry
        {

            private string[] _ignoreProperties;
            public string[] IgnoreProperties
            {
                get { return _ignoreProperties ?? (_ignoreProperties = new string[0]); }
                set { _ignoreProperties = value ?? new string[0]; }
            }

            public string FileName { get; set; }

            public abstract Type Type { get; }
        }

        public class BackupEntry : BackupBaseEntry
        {
            public object Obj { get; set; }

            public override Type Type
            {
                get { return Obj.GetType(); }
            }

        }

        public class ExtractEntry : BackupBaseEntry
        {
            public string TypeName { get; set; }

            public override Type Type
            {
                get { return Type.GetType(TypeName); }
            }
        }

        #endregion

        #region Class fields

        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private ZipArchive _zipArchive;
        private List<ExtractEntry> _extpactMap = new List<ExtractEntry>();
        private readonly List<BackupEntry> _backupEntries = new List<BackupEntry>();

        #endregion

        #region Constructor

        public Backup(IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver)
        {
            _blobStorageProvider = blobStorageProvider;
            _blobUrlResolver = blobUrlResolver;
        }

        #endregion

        #region Zip

        public void Add(string fileName, object obj, string[] ignoreList = null)
        {
            if (obj == null)
            {
                throw new Exception("obj argument is null. Can't create file from null object.");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("fileName argument is null or empty.");
            }
            _backupEntries.Add(new BackupEntry { FileName = fileName, IgnoreProperties = ignoreList, Obj = obj });
        }

        public string Save(string fileName, string folderName = "temp")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("fileName argument is null or empty.");
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
                        AddToZip(entry, zipStream);
                    }
                    AddMapToZip(zipStream);
                }

                outputMemoryStream.Position = 0;
                var uploadInfo = new UploadStreamInfo
                {
                    FileName = string.Format("{0}.zip", fileName),
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

        #region Unzip

        /// <summary>
        /// Open zip file before extract
        /// </summary>
        /// <param name="fileUrl"></param>
        public void OpenZip(string fileUrl)
        {
            _zipArchive = new ZipArchive(_blobStorageProvider.OpenReadOnly(fileUrl), ZipArchiveMode.Read, true);
            LoadMap();
        }

        /// <summary>
        /// Close zip file after extact
        /// </summary>
        public void CloseZip()
        {
            _zipArchive.Dispose();
            _extpactMap.Clear();
        }

        /// <summary>
        /// Serialize xml file to object by given file name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T GetZipObject<T>(string fileName) where T : class
        {
            ExtractEntry extractEntry = null;

            if (_extpactMap != null)
            {
                extractEntry = _extpactMap.FirstOrDefault(x => x.FileName == fileName);
            }

            extractEntry = extractEntry
                ?? new ExtractEntry
                {
                    FileName = fileName,
                    TypeName = typeof(T).AssemblyQualifiedName
                };

            return GetFromZip<T>(extractEntry);
        }

        /// <summary>
        ///  Serialize xml files to objects by given file name mask 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IEnumerable<T> GetZipObjects<T>(string fileName) where T : class
        {
            return _extpactMap
                .Where(x => x.FileName.StartsWith(fileName, StringComparison.InvariantCultureIgnoreCase))
                .Select(o => GetZipObject<T>(o.FileName))
                .ToList();
        }

        #endregion

        #region Private methods
        
        private void AddToZip(BackupEntry backupEntry, ZipOutputStream zipStream)
        {
            // Serialize BackupEntry
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream) { AutoFlush = true })
                {
                    var type = backupEntry.Obj.GetType();

                    // Prepare ignored properties 
                    var xmlOver = GetIgnoreProperties(backupEntry);
                    var serializer = new XmlSerializer(type, xmlOver);
                    serializer.Serialize(streamWriter, backupEntry.Obj);

                    //Add result to zip
                    memoryStream.Position = 0;
                    var newEntry = new ZipEntry(backupEntry.FileName) { DateTime = DateTime.Now };
                    zipStream.PutNextEntry(newEntry);
                    StreamUtils.Copy(memoryStream, zipStream, new byte[4096]);
                    zipStream.CloseEntry();
                    _extpactMap.Add(new ExtractEntry { TypeName = type.AssemblyQualifiedName, IgnoreProperties = backupEntry.IgnoreProperties, FileName = backupEntry.FileName });
                }
            }
        }

        private T GetFromZip<T>(ExtractEntry objectDefinition) where T : class
        {
            var entry = _zipArchive.Entries.FirstOrDefault(x => x.Name.Equals(objectDefinition.FileName, StringComparison.InvariantCultureIgnoreCase));

            using (var reader = entry.Open())
            {
                var type = objectDefinition.Type;

                // Prepare ignored properties 
                var xmlOver = GetIgnoreProperties(objectDefinition);
                var serializer = new XmlSerializer(type, xmlOver);
                var serializedObject = serializer.Deserialize(reader);

                return serializedObject as T;
            }
        }

        private void AddMapToZip(ZipOutputStream zipStream)
        {
            AddToZip(new BackupEntry { FileName = "map.xml", Obj = _extpactMap.ToArray() }, zipStream);
        }
 
        private void LoadMap()
        {
            var objectDefinition = new ExtractEntry
            {
                FileName = "map.xml",
                TypeName = typeof(ExtractEntry[]).AssemblyQualifiedName
            };
            _extpactMap = GetFromZip<ExtractEntry[]>(objectDefinition).ToList();
        }

        private XmlAttributeOverrides GetIgnoreProperties(BackupBaseEntry entry)
        {
            var xmlOver = new XmlAttributeOverrides();
            var xmlAttr = new XmlAttributes { XmlIgnore = true };

            var newIgnorePropertyNames = new List<string>(entry.IgnoreProperties);

            var properties = entry.Type.GetProperties();

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