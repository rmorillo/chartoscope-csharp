using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class FileCacheItem
    {
        private string  fileName;

        public string  FileName
        {
            get { return fileName; }
        }

        private CacheHeaderInfo header;

        public CacheHeaderInfo Header
        {
            get { return header; }
        }                

        private BinaryField[] fields;

        public BinaryField[] Fields
        {
            get { return fields; }
        }

        private BinaryAppender appender;

        public BinaryAppender Appender
        {
            get { return appender; }
            set { this.appender = value; }
        }

        private BinaryNavigator navigator;

        public BinaryNavigator Navigator
        {
            get { return navigator; }
            set { this.navigator = value; }
        }

        public FileCacheItem(string fileName, CacheHeaderInfo header, BinaryField[] fields)
        {
            this.fileName = fileName;
            this.header = header;
            this.fields = fields;
        }
    }
}
