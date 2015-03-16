using DropboxSync.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxSync.BL
{
    class File:IFile
    {
        private File() {
            this.Type = FileSystemType.File;
        }

        public File(System.IO.FileInfo fi):this()
        {
            this.Path = fi.FullName;
            
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
