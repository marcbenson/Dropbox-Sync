using DropboxSync.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxSync.BL
{
    class Directory:IDirectory
    {
        private Directory()
        {
            this.Type = FileSystemType.Directory;
        }

        public Directory(DirectoryInfo di):this()
        {
            this.Path = di.FullName;
        }

        public List<IFileSystemEntry> Entries
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public DateTime LastModified
        {
            get;
            set;
        }

        public IDirectory Parent
        {
            get;
            set;
        }

        public FileSystemType Type
        {
            get;
            set;
        }
    }
}
