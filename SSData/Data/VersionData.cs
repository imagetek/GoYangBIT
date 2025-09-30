using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSData
{
    public class VersionData
    {
        public int MAJOR_VER { get; set; }
        public int MINOR_VER { get; set; }
        public int BUILD_VER { get; set; }
        public List<FileData> FILES { get; set; }
        public DateTime START_YMD { get; set; }
        public DateTime END_YMD { get; set; }
    }
}
