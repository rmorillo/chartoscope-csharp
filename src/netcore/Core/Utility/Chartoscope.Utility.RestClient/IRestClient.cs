using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Utility.RestClient
{
    public interface IRestClient
    {
        Task<T> Get<T>(IGetRequest<T> request);
    }
}
