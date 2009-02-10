using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBuilder
{
    public class PBLNode
    {
        private SortedDictionary<int, PBLNodeEntry> _nodeEntry;

        internal PBLNode(PBLFile file) 
        {
            File = file;
        }

        public int OffsetOfNextLeftBlock { get; set; }
        public int OffsetOfParentBlock { get; set; }
        public int OffsetOfNextRightBlock { get; set; }
        public int SpaceLeftInBlock { get; set; }
        public int EntryCount { get; set; }
        public PBLFile File { get; set; }

        public SortedDictionary<int, PBLNodeEntry> Entries 
        {
            get
            {
                if (_nodeEntry == null)
                {
                    _nodeEntry = new SortedDictionary<int, PBLNodeEntry>();
                }
                return _nodeEntry;
            }
        }
    }
}
