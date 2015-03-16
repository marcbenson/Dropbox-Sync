using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxSync.BL.Interfaces
{
    public interface IDirectory:IFileSystemEntry
    {
        List<IFileSystemEntry> Entries { get; set; }
    }
}
