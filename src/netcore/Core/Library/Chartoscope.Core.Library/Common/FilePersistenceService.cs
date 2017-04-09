using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class FilePersistenceService
    {
        private Dictionary<string, FileWriter> _fileWriters;
        public FilePersistenceService()
        {
            _fileWriters = new Dictionary<string, FileWriter>();
        }

        public IPersistenceWriter CreateFileWriter(string fileName)
        {
            var fileWriter = new FileWriter(fileName);
            _fileWriters.Add(fileName, fileWriter);
            return fileWriter;
        }
    }
}
