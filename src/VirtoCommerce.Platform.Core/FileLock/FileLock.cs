using System;
using System.IO;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.FileLock
{
    public class FileLockContent
    {
        public Guid Token { get; set; }
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// A file lock is implemented by having multiple process look for a file with the same name at a well-known location on the file system
    /// The lock is acquired when the file is written to disk and the lock is released when it is deleted.
    /// </summary>
    public class FileLock
    {
        private readonly Guid _token;

        public FileLock(string lockFilePath, TimeSpan lockTimeout)
        {
            LockTimeout = lockTimeout;
            LockFilePath = lockFilePath;
            _token = Guid.NewGuid();
        }

        public TimeSpan LockTimeout { get; }
        public string LockFilePath { get; }

        public bool TryAcquireLock()
        {
            var retVal = true;

            if (File.Exists(LockFilePath))
            {
                try
                {
                    using (var stream = File.OpenRead(LockFilePath))
                    {
                        var lockContent = stream.DeserializeJson<FileLockContent>();
                        var lockWriteTime = new DateTime(lockContent.Timestamp);
                        //This lock belongs to this process - we can reacquire the lock
                        //or the lock has not timed out - we can't acquire it
                        retVal = lockContent.Token == _token || Math.Abs((DateTime.UtcNow - lockWriteTime).TotalSeconds) > LockTimeout.TotalSeconds;
                    }
                }
                catch (IOException)
                {
                    retVal = false;
                }
                catch (Exception)
                {
                    // Something went wrong - reacquire this lock
                }
            }

            //Acquire the lock
            if (retVal)
            {
                var lockContent = new FileLockContent
                {
                    Timestamp = DateTime.UtcNow.Ticks,
                    Token = _token
                };
                try
                {
                    using (var stream = File.Create(LockFilePath))
                    {
                        lockContent.SerializeJson(stream);
                    }
                }
                catch (Exception)
                {
                    retVal = false;
                }
            }
            return retVal;
        }

        public void ReleaseLock()
        {
            //Need to own the lock in order to release it (and we can reacquire the lock inside the current process)
            if (File.Exists(LockFilePath) && TryAcquireLock())
            {
                try
                {
                    File.Delete(LockFilePath);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
