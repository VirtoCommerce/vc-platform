using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Data.TransactionFileManager.Operations;

namespace VirtoCommerce.Platform.Data.TransactionFileManager
{
    /// <summary>
    /// port from https://github.com/rsevil/Transactions
    /// </summary>
    public class TransactionFileManager : ITransactionFileManager
    {
        /// <summary>Creates all directories in the specified path.</summary>
        /// <param name="path">The directory path to create.</param>
        public void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            if (IsInTransaction())
                EnlistOperation(new CreateDirectory(path));
            else
                Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Delete directory or file
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                if (IsInTransaction())
                    EnlistOperation(new DeleteDirectory(path));
                else
                    Directory.Delete(path, true);
            }
            else
            {
                if (IsInTransaction())
                    EnlistOperation(new DeleteFile(path));
                else
                    File.Delete(path);
            }
        }


        /// <summary>
        /// Safe deleting with all subdirectories and files
        /// </summary>
        /// <param name="path"></param>
        public void SafeDelete(string path)
        {
            if (Directory.Exists(path))
            {
                //try delete whole directory
                try
                {
                    Delete(path);
                }
                //Because some folder can be locked by ASP.NET Bundles file monitor we should ignore IOException
                catch (IOException)
                {
                    //If fail need to delete directory content first
                    //Files                 
                    foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                    {
                        Delete(file);
                    }
                    //Dirs
                    foreach (var subDirectory in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            Delete(subDirectory);
                        }
                        catch (IOException)
                        {
                        }
                    }
                    //Then try to delete main directory itself
                    try
                    {
                        Delete(path);
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }


        /// <summary>Dictionary of transaction enlistment objects for the current thread.</summary>
        [ThreadStatic]
        private static Dictionary<string, TransactionFileManagerEnlistment> _enlistments;

        private static readonly object _enlistmentsLock = new object();

        private static bool IsInTransaction()
        {
            return Transaction.Current != null;
        }

        private static void EnlistOperation(IRollbackableOperation operation)
        {
            Transaction transaction = Transaction.Current;
            TransactionFileManagerEnlistment enlistment;

            lock (_enlistmentsLock)
            {
                if (_enlistments == null)
                {
                    _enlistments = new Dictionary<string, TransactionFileManagerEnlistment>();
                }

                if (!_enlistments.TryGetValue(transaction.TransactionInformation.LocalIdentifier, out enlistment))
                {
                    enlistment = new TransactionFileManagerEnlistment(transaction);
                    _enlistments.Add(transaction.TransactionInformation.LocalIdentifier, enlistment);
                }
                enlistment.EnlistOperation(operation);
            }
        }
    }
}
