using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBuilder
{
    public class PBLNodeEntry
    {
        private string _comment = null;

        internal PBLNodeEntry(PBLNode node) 
        {
            ParentNode = node;
        }

        public PBLNode ParentNode { get; set; }
        public string Version { get; set; }
        public DateTime? Date { get; set; }
        public int? OffsetOfFirstDataBlock { get; set; }
        public int? ObjectSize { get; set; }
        public int? CommentLength { get; set; }
        public int? ObjectNameLength { get; set; }
        public string ObjectName { get; set; }
        public string Comment 
        {
            get
            {
                if (_comment == null)
                {
                    _comment = string.Empty;
                    int commentLength = CommentLength.HasValue ? CommentLength.Value : 0;
                    Encoding enc = this.ParentNode.File.IsUnicode ? Encoding.Unicode : Encoding.ASCII;
                    if (this.ParentNode.File.IsUnicode) commentLength *= 2;
                    if (commentLength > 0)
                    {
                        int offset = OffsetOfFirstDataBlock.Value;
                        if (ParentNode.File.Data.ContainsKey(offset))
                        {
                            PBLData dataObj = ParentNode.File.Data[offset];

                            byte[] commentBytes = new byte[commentLength];
                            Array.Copy(dataObj.Data, 0, commentBytes, 0, commentLength);
                            commentLength = 0;

                            _comment = enc.GetString(commentBytes);
                        }
                    }
                }
                return _comment;
            }
        }

        public byte[] RawData
        {
            get 
            {
                List<byte> bytes = new List<byte>();

                int offset = OffsetOfFirstDataBlock.Value;
                int commentLength = CommentLength.HasValue ? CommentLength.Value : 0;
                if (this.ParentNode.File.IsUnicode) commentLength *= 2;
                while (ParentNode.File.Data.ContainsKey(offset))
                {
                    PBLData dataObj = ParentNode.File.Data[offset];
                    if (commentLength > 0)
                    {
                        byte[] bbb = new byte[dataObj.Data.Length - commentLength];
                        Array.Copy(dataObj.Data, commentLength, bbb, 0, dataObj.Data.Length - commentLength);
                        commentLength = 0;
                        bytes.AddRange(bbb);
                    }
                    else
                    {
                        bytes.AddRange(dataObj.Data);
                    }

                    offset = dataObj.OffsetOfNextBlock;
                }

                return bytes.ToArray();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (OffsetOfFirstDataBlock.HasValue) sb.Append("[").Append(OffsetOfFirstDataBlock).Append("] ");
            if (CommentLength.HasValue) sb.Append(CommentLength).Append(" ");
            if (ObjectNameLength.HasValue) sb.Append(ObjectNameLength).Append(" ");
            sb.Append(ObjectName).Append(" ").Append(Comment);

            return sb.ToString();
        }
    }
}
