using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLSoft.Pub.Model
{
    public interface IImportObject
    {
        string MAP_ImportUpdate { get; }

        string MAP_ImportInsert { get; }
    }
}
