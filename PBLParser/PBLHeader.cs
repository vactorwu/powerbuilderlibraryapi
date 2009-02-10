using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBuilder
{
    public class PBLHeader
    {
        public string Version { get; set; }
        public DateTime CreationOptimizationDate { get; set; }
        public int OffsetOfFirstSCCBlock { get; set; }
        public int NetSizeOfSCCData { get; set; }
        public string Description { get; set; }
    }
}
