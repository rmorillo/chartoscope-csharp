using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class FileCacheReader: ICacheReader
    {
        private Dictionary<string, FileCacheItem> binaryFiles = null;
        private string cacheFolder= null;

        public FileCacheReader(string cacheFolder)
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

            binaryFiles.Add(header.IdentityCode,  new FileCacheItem(fileName, header, fields));
        }

        public void BeginReading(string identityCode)
        {
            binaryFiles[identityCode].Navigator = new BinaryNavigator(binaryFiles[identityCode].FileName, binaryFiles[identityCode].Fields);
        }

        public void EndReading(string identityCode)
        {
            binaryFiles[identityCode].Navigator.Close();
        }

        public long GetRowCount(string identityCode)
        {
            return binaryFiles[identityCode].Navigator.RowCount;
        }

        public CacheRow Find(string identityCode, string searchKey)
        {
            CacheRow cacheRow = null;
            long lastRowNumber = 0;
            if (binaryFiles.ContainsKey(identityCode))
            {
                BinaryNavigator navigator = binaryFiles[identityCode].Navigator;
                navigator.MoveToFirst();
                object[] row = null;
                bool found = false;
                while (!navigator.EOF())
                {
                    lastRowNumber = navigator.RowNumber;
                    row = navigator.Read();
                    if (((long)row[0]).ToString() == searchKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {                    
                    cacheRow= new CacheRow(binaryFiles[identityCode].Header.Columns, lastRowNumber, row);
                }
            }

            return cacheRow;
        }


        public CacheHeaderInfo RequestHeader(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType)
        {
            string fileName = FileHelper.CreateCacheFileName(cacheFolder, identityCode, barType.Code, cacheId, cacheType);
            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));

            int dataPosition = 0;
            CacheHeaderInfo headerInfo = FileHelper.ReadBinaryFileHeader(reader, ref dataPosition);

            reader.Close();
            return headerInfo;
        }


        public CacheRow[] Select(string identityCode, long rowNumber, long rows = 1)
        {
            List<CacheRow> result = new List<CacheRow>();
            BinaryNavigator navigator= binaryFiles[identityCode].Navigator;            
            if (navigator.Seek(rowNumber))
            {
                for (int index = 0; index < rows; index++)
                {
                    result.Add(new CacheRow(binaryFiles[identityCode].Header.Columns, rowNumber + index, navigator.Read()));
                    if (navigator.EOF())
                    {
                        break;
                    }
                }
            }

            return result.Count > 0 ? result.ToArray() : null;
        }

        public CacheRow[] Next(string identityCode, long rows = 1)
        {
            List<CacheRow> cacheRowList = new List<CacheRow>();
            long rowNumber = binaryFiles[identityCode].Navigator.RowNumber;
            for (int index = 0; index < rows; index++)
            {
                cacheRowList.Add(new CacheRow(binaryFiles[identityCode].Header.Columns, rowNumber + index, binaryFiles[identityCode].Navigator.Read()));
            }
            return cacheRowList.Count>0? cacheRowList.ToArray(): null;
        }

        public CacheRow NextItem(string identityCode)
        {
            long rowNumber = binaryFiles[identityCode].Navigator.RowNumber;
            object[] result= binaryFiles[identityCode].Navigator.Read();

            return result==null? null: new CacheRow(binaryFiles[identityCode].Header.Columns, rowNumber, result);
        }
    }
}
