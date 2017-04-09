using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Utility.RestClient
{
    public class JsonWebStream<T> where T: IHeartbeat
    {        
        private JsonWebSession _session;
        private bool _shutdown;
        private string _path;
        public delegate void DataHandler(T data);

        public event DataHandler DataReceived;
        public JsonWebStream(string path, JsonWebSession session)
        {
            _session = session;
            _path = path;        
        }

        public async Task BeginReceiveStreamAsync()
        {
            WebResponse response = await _session.GetWebResponseAsync(_path);

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {                
                _shutdown = false;

                await Task.Run(() =>
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                    while (!_shutdown)
                    {
                        MemoryStream memStream = new MemoryStream();

                        string line = reader.ReadLine();
                        memStream.Write(Encoding.UTF8.GetBytes(line), 0, Encoding.UTF8.GetByteCount(line));
                        memStream.Position = 0;

                        var data = (T)serializer.ReadObject(memStream);

                    // Don't send heartbeats
                    if (!data.IsHeartbeat())
                        {
                            OnDataReceived(data);
                        }
                    }
                }
                );
            }
        }

        public void EndReceiveStream()
        {
            _shutdown = true;
        }

        public void OnDataReceived(T data)
        {
            DataReceived?.Invoke(data);
        }
    }
}
