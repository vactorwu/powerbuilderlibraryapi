using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace PowerBuilder
{
    public class PBLParser
    {
        public const int BLOCK_SIZE = 512;
        public const int BLOCK_SIZE_UNICODE = BLOCK_SIZE*2;
        public const int NODE_BLOCK_COUNT = 6;
        public const string HEADER_BLOCK_PREFIX = "HDR*";
        public const string FREE_USED_BLOCK_PREFIX = "FRE*";
        public const string NODE_BLOCK_PREFIX = "NOD*";
        public const string OBJECT_DATA_BLOCK_PREFIX = "DAT*";
        public const string TRAILER_BLOCK_PREFIX = "TRL*";
        public const string POWERBUILDER = "PowerBuilder";

        public static PBLHeader ParseHeader(string filename)
        {
            FileInfo file = new FileInfo(filename);
            PBLFile pbl = new PBLFile(file);

            byte[] longBytes = new byte[4];
            byte[] intBytes = new byte[2];

            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    ParseHeaderIsValid(pbl, reader);
                    reader.Close();
                }
            }
            return pbl.Header;
        }


        public static void ShowBlockHeaders(string filename, int blockSize)
        {
            FileInfo file = new FileInfo(filename);
            PBLFile pbl = new PBLFile(file);

            byte[] longBytes = new byte[4];
            byte[] intBytes = new byte[2];
            int blockOffset = 0;
            int lastReadBlockSize = 0;

            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    byte[] block = null;
                    do
                    {
                        blockOffset += lastReadBlockSize;
                        block = reader.ReadBytes(blockSize);
                        lastReadBlockSize = blockSize;
                        if (block == null || block.Length < 4) break;

                        byte[] blockHdr = new byte[4];

                        Array.Copy(block, 0, blockHdr, 0, 4);

                        string prefix = ASCIIEncoding.ASCII.GetString(blockHdr);

                        Console.Write(prefix);
                        Console.Write(" => ");
                        foreach (byte b in blockHdr) Console.Write("{0:X}", b);
                        Console.WriteLine();

                    } while (block != null);
                }
            }
        }

        public static PBLFile Parse(string filename)
        {
            FileInfo file = new FileInfo(filename);
            PBLFile pbl = new PBLFile(file);

            bool isUnicode = false;
            byte[] longBytes = new byte[4];
            byte[] intBytes = new byte[2];

            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    if (ParseHeaderIsValid(pbl, reader))
                    {
                        if (pbl.IsUnicode)
                            ParseUnicode(pbl, reader);
                        else
                            ParseASCII(pbl, reader);
                    }
                }
            }
            return pbl;
        }

        private static bool ParseHeaderIsValid(PBLFile pbl, BinaryReader reader)
        {
            pbl.IsUnicode = false;
            bool isPBL = false;

            byte[] blockHdr = new byte[4];
            byte[] block = reader.ReadBytes(BLOCK_SIZE);

            Array.Copy(block, 0, blockHdr, 0, 4);

            string prefix = ASCIIEncoding.ASCII.GetString(blockHdr);
            if (prefix == HEADER_BLOCK_PREFIX)
            {
                isPBL = true;

                byte[] powerBuilderAscii = new byte[12];
                Array.Copy(block, 4, powerBuilderAscii, 0, 12);
                string pbString = ASCIIEncoding.ASCII.GetString(powerBuilderAscii);
                if (pbString != POWERBUILDER)
                {
                    byte[] powerBuilderUnicode = new byte[24];
                    Array.Copy(block, 4, powerBuilderUnicode, 0, 24);
                    pbString = ASCIIEncoding.Unicode.GetString(powerBuilderUnicode);
                    if (pbString == POWERBUILDER)
                    {
                        pbl.IsUnicode = true;
                    }
                }

                if (!pbl.IsUnicode)
                {
                    byte[] versionBytes = new byte[4];
                    Array.Copy(block, 18, versionBytes, 0, 4);
                    pbl.Header.Version = ASCIIEncoding.ASCII.GetString(versionBytes).TrimEnd('\0');

                    byte[] creationOptimizationTimeBytes = new byte[4];
                    Array.Copy(block, 22, creationOptimizationTimeBytes, 0, 4);
                    int i = System.BitConverter.ToInt32(creationOptimizationTimeBytes, 0);
                    pbl.Header.CreationOptimizationDate = ConvertFromUnixTimestamp(i);

                    byte[] descriptionBytes = new byte[255];
                    Array.Copy(block, 28, descriptionBytes, 0, 255);
                    pbl.Header.Description = ASCIIEncoding.ASCII.GetString(descriptionBytes).TrimEnd('\0');

                    byte[] sccDataBlockOffsetBytes = new byte[4];
                    Array.Copy(block, 284, sccDataBlockOffsetBytes, 0, 4);
                    pbl.Header.OffsetOfFirstSCCBlock = System.BitConverter.ToInt32(sccDataBlockOffsetBytes, 0);

                    byte[] sccDataNetSizeBytes = new byte[4];
                    Array.Copy(block, 288, sccDataNetSizeBytes, 0, 4);
                    pbl.Header.NetSizeOfSCCData = System.BitConverter.ToInt32(sccDataNetSizeBytes, 0);
                }
                else
                {
                    byte[] block2 = reader.ReadBytes(BLOCK_SIZE);
                    byte[] blockBig = new byte[BLOCK_SIZE_UNICODE];
                    Array.Copy(block, 0, blockBig, 0, BLOCK_SIZE);
                    Array.Copy(block2, 0, blockBig, BLOCK_SIZE, BLOCK_SIZE);

                    block = blockBig;

                    byte[] versionBytes = new byte[8];
                    Array.Copy(block, 32, versionBytes, 0, 8);
                    pbl.Header.Version = UnicodeEncoding.Unicode.GetString(versionBytes).TrimEnd('\0');

                    byte[] creationOptimizationTimeBytes = new byte[4];
                    Array.Copy(block, 40, creationOptimizationTimeBytes, 0, 4);
                    int i = System.BitConverter.ToInt32(creationOptimizationTimeBytes, 0);
                    pbl.Header.CreationOptimizationDate = ConvertFromUnixTimestamp(i);

                    byte[] descriptionBytes = new byte[510];
                    Array.Copy(block, 44, descriptionBytes, 0, 510);
                    pbl.Header.Description = UnicodeEncoding.Unicode.GetString(descriptionBytes).TrimEnd('\0');

                    byte[] sccDataBlockOffsetBytes = new byte[4];
                    Array.Copy(block, 558, sccDataBlockOffsetBytes, 0, 4);
                    pbl.Header.OffsetOfFirstSCCBlock = System.BitConverter.ToInt32(sccDataBlockOffsetBytes, 0);

                    byte[] sccDataNetSizeBytes = new byte[4];
                    Array.Copy(block, 562, sccDataNetSizeBytes, 0, 4);
                    pbl.Header.NetSizeOfSCCData = System.BitConverter.ToInt32(sccDataNetSizeBytes, 0);
                }
            }

            return isPBL;
        }

        public static void ParseASCII(PBLFile pbl, BinaryReader reader)
        {
            byte[] longBytes = new byte[4];
            byte[] intBytes = new byte[2];
            int blockOffset = 0;
            int lastReadBlockSize = BLOCK_SIZE;

            byte[] block = null;
            do
            {
                blockOffset += lastReadBlockSize;
                block = reader.ReadBytes(BLOCK_SIZE);
                lastReadBlockSize = BLOCK_SIZE;
                if (block == null || block.Length < 4) break;

                byte[] blockHdr = new byte[4];

                Array.Copy(block, 0, blockHdr, 0, 4);

                string prefix = ASCIIEncoding.ASCII.GetString(blockHdr);

                switch (prefix)
                {
                    case FREE_USED_BLOCK_PREFIX:
                        PBLBitmap bmp = new PBLBitmap();
                        bmp.OffsetOfNextBlock = System.BitConverter.ToInt32(block, 4);

                        byte[] dataBlockBitMap = new byte[504];
                        Array.Copy(block, 8, dataBlockBitMap, 0, 504);

                        bmp.Bitmap = new BitArray(dataBlockBitMap);
                        pbl.Bitmaps[blockOffset] = bmp;

                        break;
                    case OBJECT_DATA_BLOCK_PREFIX:
                        PBLData data = new PBLData();
                        pbl.Data[blockOffset] = data;

                        data.OffsetOfNextBlock = System.BitConverter.ToInt32(block, 4);
                        data.LengthOfData = System.BitConverter.ToInt16(block, 8);


                        byte[] objDataBytes = new byte[data.LengthOfData];
                        Array.Copy(block, 10, objDataBytes, 0, data.LengthOfData);
                        data.Data = objDataBytes;

                        break;
                    case NODE_BLOCK_PREFIX:
                        byte[] bigChunk = new byte[BLOCK_SIZE * NODE_BLOCK_COUNT];
                        byte[] secondPart = reader.ReadBytes(BLOCK_SIZE * (NODE_BLOCK_COUNT - 1));
                        lastReadBlockSize += BLOCK_SIZE * (NODE_BLOCK_COUNT - 1);
                        block.CopyTo(bigChunk, 0);
                        secondPart.CopyTo(bigChunk, BLOCK_SIZE);

                        pbl.Node.OffsetOfNextLeftBlock = System.BitConverter.ToInt32(block, 4);
                        pbl.Node.OffsetOfParentBlock = System.BitConverter.ToInt32(block, 8);
                        pbl.Node.OffsetOfNextRightBlock = System.BitConverter.ToInt32(block, 12);
                        pbl.Node.SpaceLeftInBlock = System.BitConverter.ToInt16(block, 16);
                        pbl.Node.EntryCount = System.BitConverter.ToInt16(block, 20);

                        int index = 32;
                        for (int i = 0; i < 7; i++)
                        {
                            PBLNodeEntry nodeEntry = new PBLNodeEntry(pbl.Node);
                            pbl.Node.Entries[blockOffset + index] = nodeEntry;

                            Array.Copy(block, index, longBytes, 0, 4);
                            string childPrefix = ASCIIEncoding.ASCII.GetString(longBytes).TrimEnd('\0');
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            nodeEntry.Version = ASCIIEncoding.ASCII.GetString(longBytes).TrimEnd('\0');
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            nodeEntry.OffsetOfFirstDataBlock = System.BitConverter.ToInt32(longBytes, 0);
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            nodeEntry.ObjectSize = System.BitConverter.ToInt32(longBytes, 0);
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            int dateLong = System.BitConverter.ToInt32(longBytes, 0);
                            nodeEntry.Date = ConvertFromUnixTimestamp(dateLong);
                            index += 4;

                            Array.Copy(block, index, intBytes, 0, 2);
                            nodeEntry.CommentLength = System.BitConverter.ToInt16(intBytes, 0);
                            index += 2;

                            Array.Copy(block, index, intBytes, 0, 2);
                            nodeEntry.ObjectNameLength = System.BitConverter.ToInt16(intBytes, 0);
                            index += 2;

                            byte[] objNameBytes = new byte[nodeEntry.ObjectNameLength.Value];
                            Array.Copy(block, index, objNameBytes, 0, nodeEntry.ObjectNameLength.Value);
                            nodeEntry.ObjectName = ASCIIEncoding.ASCII.GetString(objNameBytes).TrimEnd('\0');
                            index += nodeEntry.ObjectNameLength.Value;
                        }
                        break;
                }
            } while (block != null);
        }

        public static void ParseUnicode(PBLFile pbl, BinaryReader reader)
        {
            byte[] eightBytes = new byte[8];
            byte[] longBytes = new byte[4];
            byte[] intBytes = new byte[2];
            int blockOffset = 0;
            int lastReadBlockSize = BLOCK_SIZE_UNICODE;

            byte[] block = null;
            do
            {
                blockOffset += lastReadBlockSize;
                block = reader.ReadBytes(BLOCK_SIZE);
                lastReadBlockSize = BLOCK_SIZE;
                if (block == null || block.Length < 4) break;

                byte[] blockHdr = new byte[4];

                Array.Copy(block, 0, blockHdr, 0, 4);

                string prefix = ASCIIEncoding.ASCII.GetString(blockHdr);

                switch (prefix)
                {
                    case FREE_USED_BLOCK_PREFIX:
                        PBLBitmap bmp = new PBLBitmap();
                        bmp.OffsetOfNextBlock = System.BitConverter.ToInt32(block, 4);

                        byte[] dataBlockBitMap = new byte[504];
                        Array.Copy(block, 8, dataBlockBitMap, 0, 504);

                        bmp.Bitmap = new BitArray(dataBlockBitMap);
                        pbl.Bitmaps[blockOffset] = bmp;

                        break;
                    case OBJECT_DATA_BLOCK_PREFIX:
                        PBLData data = new PBLData();
                        pbl.Data[blockOffset] = data;

                        data.OffsetOfNextBlock = System.BitConverter.ToInt32(block, 4);
                        data.LengthOfData = System.BitConverter.ToInt16(block, 8);


                        byte[] objDataBytes = new byte[data.LengthOfData];
                        Array.Copy(block, 10, objDataBytes, 0, data.LengthOfData);
                        data.Data = objDataBytes;

                        break;
                    case NODE_BLOCK_PREFIX:
                        byte[] bigChunk = new byte[BLOCK_SIZE * NODE_BLOCK_COUNT];
                        byte[] secondPart = reader.ReadBytes(BLOCK_SIZE * (NODE_BLOCK_COUNT - 1));
                        lastReadBlockSize += BLOCK_SIZE * (NODE_BLOCK_COUNT - 1);
                        block.CopyTo(bigChunk, 0);
                        secondPart.CopyTo(bigChunk, BLOCK_SIZE);

                        block = bigChunk;

                        pbl.Node.OffsetOfNextLeftBlock = System.BitConverter.ToInt32(block, 4);
                        pbl.Node.OffsetOfParentBlock = System.BitConverter.ToInt32(block, 8);
                        pbl.Node.OffsetOfNextRightBlock = System.BitConverter.ToInt32(block, 12);
                        pbl.Node.SpaceLeftInBlock = System.BitConverter.ToInt16(block, 16);
                        pbl.Node.EntryCount = System.BitConverter.ToInt16(block, 20);

                        int index = 32;
                        for (int i = 0; i < pbl.Node.EntryCount; i++)
                        {
                            PBLNodeEntry nodeEntry = new PBLNodeEntry(pbl.Node);
                            pbl.Node.Entries[blockOffset + index] = nodeEntry;

                            Array.Copy(block, index, longBytes, 0, 4);
                            string childPrefix = ASCIIEncoding.ASCII.GetString(longBytes).TrimEnd('\0');
                            index += 4;

                            Array.Copy(block, index, eightBytes, 0, 8);
                            nodeEntry.Version = UnicodeEncoding.Unicode.GetString(eightBytes).TrimEnd('\0');
                            index += 8;

                            Array.Copy(block, index, longBytes, 0, 4);
                            nodeEntry.OffsetOfFirstDataBlock = System.BitConverter.ToInt32(longBytes, 0);
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            nodeEntry.ObjectSize = System.BitConverter.ToInt32(longBytes, 0);
                            index += 4;

                            Array.Copy(block, index, longBytes, 0, 4);
                            int dateLong = System.BitConverter.ToInt32(longBytes, 0);
                            nodeEntry.Date = ConvertFromUnixTimestamp(dateLong);
                            index += 4;

                            Array.Copy(block, index, intBytes, 0, 2);
                            nodeEntry.CommentLength = System.BitConverter.ToInt16(intBytes, 0);
                            index += 2;

                            Array.Copy(block, index, intBytes, 0, 2);
                            nodeEntry.ObjectNameLength = System.BitConverter.ToInt16(intBytes, 0);
                            index += 2;

                            int len = nodeEntry.ObjectNameLength.Value;
                            if (len > block.Length - index)
                            {
                                len = (block.Length - index);
                            }
                            byte[] objNameBytes = new byte[len];
                            Array.Copy(block, index, objNameBytes, 0, len);
                            nodeEntry.ObjectName = UnicodeEncoding.Unicode.GetString(objNameBytes).TrimEnd('\0');
                            index += len;
                        }
                        break;
                }
            } while (block != null);
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
