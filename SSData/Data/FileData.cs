using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
    public class FileData
    {
        public int SEQ_NO { get; set; }
        public string FILE_NM { get; set; }
        public string FILE_EXT { get; set; }
        public long FILE_SZ { get; set; }
        public string LOCAL_URL { get; set; }
        public string REMOTE_URL { get; set; }
    }
}
