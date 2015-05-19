using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class FileHelper
    {
        public static string RemoveInvalidFilePathCharacters(string filename, string replaceChar)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar);
        }

        public static string CreateCacheFileName(string cacheFolder, string identityCode, string subSection, Guid cacheId, CacheTypeOption cacheType)
        {
            var cacheFileExtension = AttributeHelper.GetAttribute<CacheFileExtension>(cacheType);
            return Path.Combine(cacheFolder, cacheId==Guid.Empty? string.Empty: cacheId.ToString(), subSection==null? string.Empty: subSection, string.Concat(RemoveInvalidFilePathCharacters(identityCode, "_"), ".", cacheFileExtension.Extension));            
        }

        public static void CheckMissingFolders(string fileName)
        {
            string[] folders = Path.GetDirectoryName(fileName).Split(Path.DirectorySeparatorChar);
            for (int index = 1; index <= folders.Length; index++)
            {
                string targetFolder = string.Join(Path.DirectorySeparatorChar.ToString(), folders, 0, index);
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
            }
        }

        public static CacheHeaderInfo ReadBinaryFileHeader(BinaryReader reader, ref int dataPosition)
        {
            CacheHeaderInfo headerInfo = null;

            byte[] headerBytes = new byte[4];
            int result= reader.Read(headerBytes, 0, 4);

            if (result > 0)
            {
                int headerOffset = 0;
                int headerSize = FileHelper.ReadInt32(headerBytes, ref headerOffset);
                if (headerSize > 0)
                {
                    dataPosition = headerSize + headerOffset;
                    byte[] header = new byte[headerSize];
                    reader.Read(header, 0, headerSize);
                    headerInfo = FileHelper.DecodeCacheHeader(header);
                }
            }

            return headerInfo;
        }

        private static byte[] EncodeExtendedProperties(Dictionary<string, string> extendedProperties)
        {
            List<byte[]> extPropByteList = new List<byte[]>();
            extPropByteList.Add(BitConverter.GetBytes(extendedProperties.Count));
            foreach (KeyValuePair<string, string> extendedProp in extendedProperties)
            {
                List<byte[]> extPropFieldList = new List<byte[]>();
                extPropFieldList.Add(EncodeString(extendedProp.Key));
                extPropFieldList.Add(EncodeString(extendedProp.Value));

                extPropByteList.Add(MergeByteList(extPropFieldList));
            }

            return MergeByteList(extPropByteList);
        }

        public static byte[] EncodeCacheHeader(CacheHeaderInfo headerInfo)
        {
            List<byte[]> headerByteList= new List<byte[]>();
            headerByteList.Add(EncodeString(headerInfo.IdentityCode));
            headerByteList.Add(headerInfo.CacheId.ToByteArray());
            headerByteList.Add(BitConverter.GetBytes((int)headerInfo.CacheType));
            headerByteList.Add(EncodeString(headerInfo.SubSection));
            headerByteList.Add(BitConverter.GetBytes(headerInfo.Columns.Count));
            foreach(CacheColumn column in headerInfo.Columns.Values)
            {
                List<byte[]> columnByteList = new List<byte[]>();

                columnByteList.Add(EncodeString(column.Name));
                columnByteList.Add(BitConverter.GetBytes(column.Size));
                columnByteList.Add(BitConverter.GetBytes(column.Index));
                columnByteList.Add(BitConverter.GetBytes((int)column.DataType)); 
                              
                if (column.ExtendedProperties != null && column.ExtendedProperties.Count > 0)
                {                    
                    columnByteList.Add(EncodeExtendedProperties(column.ExtendedProperties));
                }
                else
                {
                    columnByteList.Add(BitConverter.GetBytes((int) 0));
                }

                headerByteList.Add(MergeByteList(columnByteList));
            }


            if (headerInfo.ExtendedProperties != null && headerInfo.ExtendedProperties.Count > 0)
            {
                headerByteList.Add(EncodeExtendedProperties(headerInfo.ExtendedProperties));
            }
            else
            {
                headerByteList.Add(BitConverter.GetBytes((int)0));
            }

            byte[] headerInfoByteList = MergeByteList(headerByteList);
            byte[] headerSize= BitConverter.GetBytes(headerInfoByteList.Length);
            byte[] complete = new byte[headerInfoByteList.Length + headerSize.Length];
            headerSize.CopyTo(complete, 0);
            headerInfoByteList.CopyTo(complete, headerSize.Length);

            return complete;
        }

        public static int ReadInt32(byte[] input, ref int offset)
        {            
            int value = BitConverter.ToInt32(input, offset);
            offset += 4;
            return value;
        }

        private static string ReadString(byte[] input, ref int offset)
        {
            int size = ReadInt32(input, ref offset);            
            string value = Encoding.ASCII.GetString(input, offset, size);
            offset += size;
            return value;
        }

        private static Guid ReadGuid(byte[] input, ref int offset)
        {
            Guid value = new Guid(GetGuidBytes(input, offset));
            offset += 16;
            return value;
        }

        private static T ReadEnum<T>(byte[] input, ref int offset)
        {
            object value = ReadInt32(input, ref offset);
            return (T) value;
        }

        public static CacheHeaderInfo DecodeCacheHeader(byte[] cacheHeader)
        {
            int offset = 0;
            string identityCode = ReadString(cacheHeader, ref offset);
            Guid cacheId = ReadGuid(cacheHeader, ref offset);
            CacheTypeOption cacheType = ReadEnum<CacheTypeOption>(cacheHeader, ref offset);
            string subSection = ReadString(cacheHeader, ref offset);
            int columnCount = ReadInt32(cacheHeader, ref offset);
            List<CacheColumn> cacheColumns = new List<CacheColumn>();
            for (int index = 0; index < columnCount; index++)
            {
                string name = ReadString(cacheHeader, ref offset);                
                int size = ReadInt32(cacheHeader, ref offset);
                int columnIndex = ReadInt32(cacheHeader, ref offset);
                CacheDataTypeOption dataType = ReadEnum<CacheDataTypeOption>(cacheHeader, ref offset);
                int extendPropertiesCount = ReadInt32(cacheHeader, ref offset);
                Dictionary<string, string> extendedProperties = ReadExtendedProperties(cacheHeader, extendPropertiesCount, ref offset);
                
                cacheColumns.Add(new CacheColumn(columnIndex, name, dataType, size, extendedProperties));
            }

            int headerExtPropCount = ReadInt32(cacheHeader, ref offset);
            Dictionary<string, string> headerExtProperties = ReadExtendedProperties(cacheHeader, headerExtPropCount, ref offset);

            return new CacheHeaderInfo(identityCode, subSection, cacheId, cacheType, cacheColumns.ToArray(), headerExtProperties);
        }

        private static Dictionary<string, string> ReadExtendedProperties(byte[] input, int propertyCount, ref int offset)
        {
            if (propertyCount > 0)
            {
                Dictionary<string, string> extendedProperties = new Dictionary<string, string>();
                for (int subIndex = 0; subIndex < propertyCount; subIndex++)
                {
                    string propertyName = ReadString(input, ref offset);
                    string propertyValue = ReadString(input, ref offset);
                    extendedProperties.Add(propertyName, propertyValue);
                }

                return extendedProperties;
            }
            else
                return null;
        }

        public static byte[] GetGuidBytes(byte[] input, int offset)
        {
            byte[] guidBytes = new byte[16];
            for (int byteIndex = 0; byteIndex < 16; byteIndex++)
            {
                guidBytes[byteIndex] = input[offset + byteIndex];
            }
            return guidBytes;
        }

        private static byte[] MergeByteList(List<byte[]> byteList)
        {
            byte[] merged = new byte[GetTotalByteLength(byteList)];
            int offset = 0;
            foreach (byte[] item in byteList)
            {
                item.CopyTo(merged, offset);
                offset += item.Length;
            }
            return merged;
        }

        private static int GetTotalByteLength(List<byte[]> byteList)
        {
            int length = 0;
            foreach (byte[] item in byteList)
            {
                length += item.Length;
            }

            return length;
        }

        private static byte[] EncodeString(string value)
        {
            byte[] encodedString = BitConverter.GetBytes((int) 0);

            if (value != null)
            {
                byte[] stringBytes = Encoding.ASCII.GetBytes(value);
                byte[] size = BitConverter.GetBytes(stringBytes.Length);
                encodedString = new byte[stringBytes.Length + size.Length];
                size.CopyTo(encodedString, 0);
                stringBytes.CopyTo(encodedString, size.Length);
            }
            return encodedString;
        }
    }
}
