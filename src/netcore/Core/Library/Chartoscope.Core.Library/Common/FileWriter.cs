using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class FileWriter: IPersistenceWriter
    {
        private string _fileName;
        private string _cacheFile;
        private FileStream _fileStream;
        private bool fileChanged = false;
        public FileWriter(string fileName)
        {
            _fileName = fileName;
        }

        public void Append(byte[] data)
        {
            _fileStream.Write(data, 0, data.Length);
            fileChanged = true;
        }

        public void Initialize(byte[] header)
        {
            _cacheFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            _fileStream = File.Create(_cacheFile);
            _fileStream.Write(header, 0, header.Length);
            fileChanged = true;
        }

        private void Save()
        {
            _fileStream.Dispose();
            File.Copy(_cacheFile, _fileName);
            fileChanged = false;
        }        

        public void Close()
        {
            if (fileChanged)
            {
                Save();
            }

            File.Delete(_cacheFile);
        }
    }
}
