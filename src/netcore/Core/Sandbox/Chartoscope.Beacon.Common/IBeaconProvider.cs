using System;
using System.Threading.Tasks;

namespace Chartoscope.Beacon.Common
{
    public interface IBeaconProvider
    {
        void Initialize();
        void Startup();
        void Shutdown();
        IInstrumentNameResolver InstrumentNameResolver { get; }

        event DelegateDefinitions.TickDataHandler TickReceived;
    }
}
