using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxSync.BL.Interfaces
{
    public interface IFileSystemEntry
    {
        Guid Id { get; set; }
        string Path { get; set; }
        DateTime LastModified { get; set; }
        IDirectory Parent { get; set; }
        FileSystemType Type { get; set; }
    }
}
