using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Chartoscope.Utility.RestClient
{
    public class JsonGetRequest<T>: IGetRequest<T>
    {
        private DataContractJsonSerializer _serializer;

        public JsonGetRequest(string path)
        {
            _path = path;
            _serializer = new DataContractJsonSerializer(typeof(T));
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        
        public T Deserialize(Stream stream)
        {
            return (T) _serializer.ReadObject(stream);
        }

    }
}
