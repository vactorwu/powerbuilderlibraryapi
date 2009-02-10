using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PowerBuilder
{
    public class PBLFile
    {
        private bool _isUnicode = false;
        private PBLHeader _pblHeader = new PBLHeader();
        private PBLNode _pblNode;
        private FileInfo _file;
        private SortedDictionary<int, PBLData> _data;
        private SortedDictionary<int, PBLBitmap> _bitmaps;

        public PBLFile(FileInfo file)
        {
            _file = file;
        }

        public bool IsUnicode
        {
            get
            {
                return _isUnicode;
            }
            set
            {
                _isUnicode = value;
            }
        }

        public PBLHeader Header
        {
            get
            {
                return _pblHeader;
            }
        }

        public PBLNode Node
        {
            get
            {
                if (_pblNode == null) _pblNode = new PBLNode(this);
                return _pblNode;
            }
        }

        public SortedDictionary<int, PBLData> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new SortedDictionary<int, PBLData>();
                }
                return _data;
            }
        }

        public SortedDictionary<int, PBLBitmap> Bitmaps
        {
            get
            {
                if (_bitmaps == null)
                {
                    _bitmaps = new SortedDictionary<int, PBLBitmap>();
                }
                return _bitmaps;
            }
        }
    }
}
