using System;
using System.Collections.Generic;
using System.Linq;

namespace LAOffsetUpdater
{
    public class AddressObject
    {
        public string Name { get; set; }
        public ScanType ScanType { get; set; }
        public ValueType ValueType { get; set; }
        public string StringSearch { get; set; }
        public byte[] BytePatternSearch { get; set; }
        public string[] BytePatternMask { get; set; }
        public long[] ResultOffset { get; set; }
        public int Pick { get; set; }
        public int AddOffset { get; set; }

        public List<long> Results { get; set; }

        public AddressObject(string name, ScanType scanType, ValueType valueType, string stringSearch,
            string bytePatternSearch, string bytePatternMask, long[] resultOffset, int pick, int addOffset = 0x00)
        {
            Name = name;
            ScanType = scanType;
            ValueType = valueType;
            StringSearch = stringSearch;
            BytePatternSearch = TransformBytePattern(bytePatternSearch);
            BytePatternMask = TransformMask(bytePatternMask);

            if (resultOffset == null)
            {
                ResultOffset = new long[]{0x00};
            }
            else
            {
                ResultOffset = resultOffset;
            }

            Pick = pick;
            AddOffset = addOffset;

            Results = new List<long>();
        }

        private byte[] TransformBytePattern(string pattern)
        {
            if (pattern == null)
                return null;

            var split = pattern.TrimStart('\\').Replace("x", "").Split('\\');

            byte[] buffer = new byte[split.Length];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(split[i], 16);
            }

            return buffer;
        }

        private string[] TransformMask(string mask)
        {
            if (mask == null)
                return null;

            string[] buffer = new string[mask.Length];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = mask.Substring(i, 1);
            }

            return buffer;
        }

        public void Find(MyProcess p)
        {
            switch (ScanType)
            {
                    case ScanType.StringSearch:
                    var stringAdr = p.ASCII_StringReferences.FirstOrDefault(x => x.Value.Name == StringSearch);

                    if (stringAdr.Value != null)
                    {
                        var refs = stringAdr.Value.Usage;

                        if (refs.Count > 0)
                        {
                            switch (ValueType)
                            {
                                    case ValueType.Address:
                                    foreach (var r in refs)
                                    {
                                        if (ResultOffset.Length <= 1)
                                        {
                                            var result = p.GetQWordPointerValue(r + ResultOffset[0]);
                                            Results.Add(result);
                                        }
                                        else
                                        {
                                            var result = p.GetQWordPointerValue(r, ResultOffset);
                                            Results.Add(result);
                                        }
                                    }
                                    break;

                                    case ValueType.InsideFunction:
                                    foreach (var r in refs)
                                    {
                                        if (ResultOffset.Length <= 1)
                                        {
                                            Results.Add(r + ResultOffset[0]);
                                        }
                                        else
                                        {
                                            Results.Add(0);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                    case ScanType.BytePatternSearch:
                    var results = p.FindBytePattern(BytePatternSearch, BytePatternMask);

                    switch (ValueType)
                    {
                            case ValueType.Address:
                            foreach (var result in results)
                            {
                                if (ResultOffset.Length <= 1)
                                {
                                    Results.Add(p.GetQWordPointerValue(result + ResultOffset[0]));
                                }
                                else
                                {
                                    Results.Add(p.GetQWordPointerValue(result, ResultOffset));
                                }
                            }
                            break;

                            case ValueType.InsideFunction:
                            foreach (var result in results)
                            {
                                if (ResultOffset.Length <= 1)
                                {
                                    Results.Add(result + ResultOffset[0]);
                                }
                                else
                                {
                                    Results.Add(0);
                                }
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
