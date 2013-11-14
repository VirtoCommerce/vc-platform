using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Waf.Applications.Services;
using System.Windows;
using Microsoft.Win32;

namespace System.Waf.VirtoCommerce.ManagementClient.Services
{
    /// <summary>
    /// This is the default implementation of the <see cref="IFileDialogService"/> interface. It shows an open or save file dialog box.
    /// </summary>
    /// <remarks>
    /// If the default implementation of this service doesn't serve your need then you can provide your own implementation.
    /// </remarks>
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public FileDialogResult ShowOpenFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            OpenFileDialog dialog = new OpenFileDialog();

            return ShowFileDialog(owner, dialog, fileTypes, defaultFileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public FileDialogResult ShowSaveFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            SaveFileDialog dialog = new SaveFileDialog();

            return ShowFileDialog(owner, dialog, fileTypes, defaultFileType, defaultFileName);
        }

        private static FileDialogResult ShowFileDialog(object owner, FileDialog dialog, IEnumerable<FileType> fileTypes, 
            FileType defaultFileType, string defaultFileName)
        {
            int filterIndex = fileTypes.ToList().IndexOf(defaultFileType);
            if (filterIndex >= 0) { dialog.FilterIndex = filterIndex + 1; }
            if (!string.IsNullOrEmpty(defaultFileName))
            {
                dialog.FileName = Path.GetFileName(defaultFileName);
                string directory = Path.GetDirectoryName(defaultFileName);
                if (!string.IsNullOrEmpty(directory))
                {
                    dialog.InitialDirectory = directory;
                }
            }

            dialog.Filter = CreateFilter(fileTypes);
            if (dialog.ShowDialog(owner as Window) == true)
            {
                filterIndex = dialog.FilterIndex - 1;
                if (filterIndex >= 0 && filterIndex < fileTypes.Count())
                {
                    defaultFileType = fileTypes.ElementAt(filterIndex);
                }
                else
                {
                    defaultFileType = null;
                }
                return new FileDialogResult(dialog.FileName, defaultFileType);
            }
            else
            {
                return new FileDialogResult();
            }
        }

        private static string CreateFilter(IEnumerable<FileType> fileTypes)
        {
            string filter = "";
            foreach (FileType fileType in fileTypes)
            {
                if (!String.IsNullOrEmpty(filter)) { filter += "|"; }
                filter += fileType.Description + "|*" + fileType.FileExtension;
            }
            return filter;
        }
    }
}
