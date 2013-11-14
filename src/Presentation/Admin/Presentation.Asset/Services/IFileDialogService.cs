using System.Collections.Generic;

namespace System.Waf.Applications.Services
{
    /// <summary>
    /// This service allows a user to specify a filename to open or save a file.
    /// </summary>
    /// <remarks>
    /// This interface is designed for simplicity. If you have to accomplish more advanced
    /// scenarios then we recommend implementing your own specific file dialog service.
    /// </remarks>
    public interface IFileDialogService
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
        FileDialogResult ShowOpenFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName);

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
        FileDialogResult ShowSaveFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName);
    }
}
