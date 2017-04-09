using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chartoscope.Utility.RestClient
{
    public interface IGetRequest<T>
    {
        T Deserialize(Stream stream);

        string Path { get; set; }
    }
}
