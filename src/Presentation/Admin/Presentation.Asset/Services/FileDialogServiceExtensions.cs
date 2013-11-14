using System.Collections.Generic;

namespace System.Waf.Applications.Services
{
    /// <summary>
    /// Provides method overloads for the <see cref="IFileDialogService"/> to simplify its usage.
    /// </summary>
    public static class FileDialogServiceExtensions
    {
        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(null, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(owner, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(null, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(owner, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(null, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(owner, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes, 
            FileType defaultFileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(null, fileTypes, defaultFileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(null, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(owner, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(null, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileType">The supported file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(owner, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(null, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(owner, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file Channels.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">service must not be null.</exception>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes, 
            FileType defaultFileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(null, fileTypes, defaultFileType, defaultFileName);
        }
    }
}
