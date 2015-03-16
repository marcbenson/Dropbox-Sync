using DropboxSync.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DropboxSync.BL
{
    public class FileSystemScanner
    {
        private int _FoundDirectoryCount;
        private int _FoundFileCount;
        private DateTime _StartTime;
        private Directory Dir;

        public string Root { get; set; }
        public ScannerStatus Status { get; set; }

       

        public FileSystemScanner(string path)
        {
            this.Root = path;

            Status = ScannerStatus.Ready;
        }

        public void Start()
        {
            if (this.Status != ScannerStatus.Ready)
                throw new ScannerNotReadyException();
            
            Status = ScannerStatus.Scanning;

            _StartTime = DateTime.UtcNow;

            ScanDirectory(this.Root);            
        }

        public void Reset()
        {
            _FoundDirectoryCount = 0;
            _FoundFileCount = 0;

            this.Status = ScannerStatus.Ready;
        }

        private void ScanDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            var dirs = di.EnumerateDirectories().OrderBy(d => d.FullName);
            var files = di.EnumerateFiles().OrderBy(f => f.FullName);

            foreach (FileInfo file in files)
            {
                _FoundFileCount++;
                
                FileSystemScanProgressEventArgs args = new FileSystemScanProgressEventArgs()
                {
                    FileSystemEntry = new File(file)
                };

                OnFileSystemScanProgress(args);
            }

            foreach (DirectoryInfo directory in dirs)
            {
                _FoundDirectoryCount++;
                FileSystemScanProgressEventArgs args = new FileSystemScanProgressEventArgs()
                {
                   FileSystemEntry = new Directory(directory)
                };

                OnFileSystemScanProgress(args);

                ScanDirectory(directory.FullName);
            }

            if (path == this.Root)
            {
                FileSystemScanCompleteEventArgs args = new FileSystemScanCompleteEventArgs()
                 {
                     TotalDirectories = _FoundDirectoryCount,
                     TotalFiles = _FoundFileCount,
                     StartTime = _StartTime,
                     EndTime = DateTime.UtcNow
                 };

                OnFileSystemScanComplete(args);

                this.Status = ScannerStatus.Complete;
            }
        }

        protected virtual void OnFileSystemScanProgress(FileSystemScanProgressEventArgs e)
        {
            FileSystemScanProgressEventHandler handler = FileSystemScanProgress;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnFileSystemScanComplete(FileSystemScanCompleteEventArgs e)
        {
            FileSystemScanCompleteEventHandler handler = FileSystemScanComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event FileSystemScanProgressEventHandler FileSystemScanProgress;
        public event FileSystemScanCompleteEventHandler FileSystemScanComplete;
    }

    public class FileSystemScanProgressEventArgs : EventArgs
    {
        public IFileSystemEntry FileSystemEntry { get; set; }

        public string Hash
        {
            get
            {
                return CalculateMD5Hash(this.FileSystemEntry.Path);
            }
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public class FileSystemScanCompleteEventArgs : EventArgs
    {
        public int TotalDirectories { get; set; }
        public int TotalFiles { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration
        {
            get
            {
                return this.EndTime.Subtract(this.StartTime);
            }
        }

        
    }

    public delegate void FileSystemScanProgressEventHandler(Object sender, FileSystemScanProgressEventArgs e);
    public delegate void FileSystemScanCompleteEventHandler(Object sender, FileSystemScanCompleteEventArgs e);
}
