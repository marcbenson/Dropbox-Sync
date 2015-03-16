using DropboxSync.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dropbox_Sync
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemScanner syncService = new FileSystemScanner(@"\\nas\media");
            syncService.FileSystemScanProgress += syncService_FileSystemScanProgress;
            syncService.FileSystemScanComplete += syncService_FileSystemScanComplete;

            try
            {
                syncService.Start();
            }
            catch (ScannerNotReadyException e)
            {
                Console.WriteLine("The Scanner is not ready");
            }
            
            Console.Read();
        }

        static void syncService_FileSystemScanComplete(object sender, FileSystemScanCompleteEventArgs e)
        {
            Console.WriteLine(String.Format("Scan Complete: Found {0} Directories and {1} Files in {2} seconds", e.TotalDirectories, e.TotalFiles,e.Duration.TotalSeconds));
        }

        static void syncService_FileSystemScanProgress(object sender, FileSystemScanProgressEventArgs e)
        {
            Console.WriteLine(e.Hash);

            
        }
    }
}
