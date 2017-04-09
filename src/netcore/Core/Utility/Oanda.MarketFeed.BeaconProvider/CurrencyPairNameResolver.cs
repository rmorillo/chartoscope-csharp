using System;
using System.Collections.Generic;
using System.Text;
using Chartoscope.Beacon.Common;
using Oanda.Common.Data;
using System.Linq;

namespace Oanda.MarketFeed.BeaconProvider
{
    public class CurrencyPairNameResolver : IInstrumentNameResolver
    {
        private IEnumerable<Instrument> _instruments;
        private Dictionary<string, InstrumentNameResolution> _resolutionCache;
        public CurrencyPairNameResolver(IEnumerable<Instrument> instruments)
        {
            _instruments = instruments;
            _resolutionCache = new Dictionary<string, InstrumentNameResolution>();
        }
        public InstrumentNameResolution Resolve(string beaconInstrumentCode)
        {
            if (_resolutionCache.ContainsKey(beaconInstrumentCode))
            {
                return _resolutionCache[beaconInstrumentCode];
            }
            else
            {                
                var instrument= _instruments.FirstOrDefault(s => s.instrument.Equals(beaconInstrumentCode));
                var resolution = new InstrumentNameResolution();
                if (instrument == null)
                {
                    resolution.Status = InstrumentNameResolutionResultOption.Failure;
                    resolution.Description = "No matching provider instrument was found.";
                }
                else
                {
                    resolution.Mapping = new InstrumentNameMapping()
                    {                        
                        BeaconInstrumentCode = beaconInstrumentCode,
                        ProviderInstrumentName = instrument.displayName,
                        ProviderInstrumentCode = instrument.instrument
                    };
                    resolution.Status = InstrumentNameResolutionResultOption.Success;
                    _resolutionCache.Add(beaconInstrumentCode, resolution);                    
                }

                return resolution;
            }
        }
    }
}
