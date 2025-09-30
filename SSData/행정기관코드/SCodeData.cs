using System;
using System.Collections.Generic;
using System.Text;

namespace SSData
{
    public class DO_CODE
    {
        public string DO_CD { get; set; }
        public string DO_NM { get; set; }
        /// <summary>
        /// 현재 사용중여부
        /// </summary>
        private bool useYN = true;
        public bool USE_YN
        {
            get { return useYN; }
            set { useYN = value; }
        }
    }

    public class SGG_CODE
    {
        public string DO_CD { get; set; }
        public string DO_NM { get; set; }
        public string SGG_CD { get; set; }
        public string SGG_NM { get; set; }
        /// <summary>
        /// 현재 사용중여부
        /// </summary>
        private bool useYN = true;
        public bool USE_YN
        {
            get { return useYN; }
            set { useYN = value; }
        }
    }
}

 
