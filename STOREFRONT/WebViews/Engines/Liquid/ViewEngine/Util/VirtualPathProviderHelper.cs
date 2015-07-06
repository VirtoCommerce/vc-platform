/* Copyright (C) 2012 by Matt Brailsford

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#region
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util
{
    public static class VirtualPathProviderHelper
    {
        #region Public Methods and Operators
        public static IEnumerable<VirtualFile> ListFiles(string virtualPath)
        {
            if (!HostingEnvironment.VirtualPathProvider.DirectoryExists(virtualPath))
            {
                return null;
            }

            var dir = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualPath);

            var files = LoadRecursive(dir.Children);
            
            return files;
        }

        private static IEnumerable<VirtualFile> LoadRecursive(IEnumerable children)
        {
            var allFiles = new List<VirtualFile>();
            var directories = children.OfType<VirtualDirectory>();
            foreach (var virtualDirectory in directories)
            {
                allFiles.AddRange(LoadRecursive(virtualDirectory.Children));
            }

            var files = children.OfType<VirtualFile>();

            if (files.Any())
            {
                allFiles.AddRange(files);
            }
            return allFiles;
        }

        public static string Load(string virtualPath)
        {
            if (!HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
            {
                return null;
            }

            try
            {
                var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath);
                using (var stream = virtualFile.Open())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static Stream Open(VirtualFile virtualFile)
        {
            using (var fs = virtualFile.Open())
            {
                var stream = new MemoryStream();
                fs.CopyTo(stream);
                stream.Position = 0;
                return stream;
            }

            /*
            using (var fs = File.OpenRead(path))
            {
                var stream = new MemoryStream();
                fs.CopyTo(stream);
                stream.Position = 0;
                return stream;
            }
             * */
        }

        public static bool FileExists(string path)
        {
            return HostingEnvironment.VirtualPathProvider.FileExists(path.Replace("/", "\\"));
        }

        public static VirtualFile GetFile(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.FileExists(virtualPath) ? HostingEnvironment.VirtualPathProvider.GetFile(virtualPath) : null;
        }

        public static string GetFileHash(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.GetFileHash(virtualPath, new [] { virtualPath });
        }
        #endregion
    }
}