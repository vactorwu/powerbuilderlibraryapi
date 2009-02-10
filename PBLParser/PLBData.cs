using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBuilder
{
    public class PBLData
    {
        public int OffsetOfNextBlock { get; set; }
        public int LengthOfData { get; set; }
        public byte[] Data { get; set; }
    }
}
