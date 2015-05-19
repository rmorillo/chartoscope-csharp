using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class FileCacheWriter: ICacheWriter
    {
        private Dictionary<string, FileCacheItem> binaryFiles = null;
        private string cacheFolder= null;

        public FileCacheWriter(string cacheFolder)
        {
            this.cacheFolder= cacheFolder;
            binaryFiles = new Dictionary<string, FileCacheItem>();
        }

        public void RegisterHeader(CacheHeaderInfo header)
        {
            string fileName = FileHelper.CreateCacheFileName(cacheFolder, header.IdentityCode, header.SubSection, header.CacheId, header.CacheType);
            BinaryField[] fields = new BinaryField[header.Columns.Count];
            int index = 0;
            foreach (CacheColumn column in header.Columns.Values)
            {
                BinaryColumnTypeOption binaryColumnType = BinaryColumnTypeOption.String;
                switch (column.DataType)
                {
                    case CacheDataTypeOption.Double:
                        binaryColumnType = BinaryColumnTypeOption.Double;
                        break;
                    case CacheDataTypeOption.Long:
                        binaryColumnType = BinaryColumnTypeOption.Long;
                        break;
                    case CacheDataTypeOption.String:
                        binaryColumnType = BinaryColumnTypeOption.String;
                        break;
                    case CacheDataTypeOption.Integer:
                        binaryColumnType = BinaryColumnTypeOption.Integer;
                        break;
                    case CacheDataTypeOption.Guid:
                        binaryColumnType = BinaryColumnTypeOption.Guid;
                        break;
                }

                fields[index] = new BinaryField(column.Name, binaryColumnType, column.Size);

                index++;
            }

            if (!binaryFiles.ContainsKey(header.IdentityCode))
            {
                binaryFiles.Add(header.IdentityCode, new FileCacheItem(fileName, header, fields));
            }
        }

        public void BeginWriting(string identityCode)
        {
            binaryFiles[identityCode].Appender = new BinaryAppender(binaryFiles[identityCode].FileName, FileHelper.EncodeCacheHeader(binaryFiles[identityCode].Header), binaryFiles[identityCode].Fields);
        }

        public void EndWriting(string identityCode)
        {
            binaryFiles[identityCode].Appender.Close();
        }


        public void Write(string identityCode, object[] values)
        {
            binaryFiles[identityCode].Appender.Append(values);
        }
    }
}
