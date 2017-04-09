using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Chartoscope.Beacon.Common;
using System.Reflection;
using System.Linq;

namespace Chartoscope.Beacon.Shell
{
    public class ProviderHost
    {
        private IBeaconProvider _provider;
        public ProviderHost(string assemblyPath, string providerTypeName)
        {
            var providerAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            var providerType = providerAssembly.GetType(providerTypeName);

            LoadProvider(providerType);
        }

        public ProviderHost(Type providerType)
        {
            LoadProvider(providerType);
        }

        private void LoadProvider(Type providerType)
        {
            _provider = (IBeaconProvider)CreateInstance(providerType, true);
        }

        public IEnumerable<InstrumentNameResolution> LoadInstruments(IEnumerable<string> instruments)
        {            
            _provider.Initialize();

            List<InstrumentNameResolution> resolutions = new List<InstrumentNameResolution>();

            IInstrumentNameResolver resolver = _provider.InstrumentNameResolver;
                        
            foreach(var instrument in instruments)
            {
                resolutions.Add(resolver.Resolve(instrument));                
            }

            return resolutions;
        }

        public void Startup()
        {

        }

        public void Shutdown()
        {

        }

        private object CreateInstance(Type type, bool genParam)
        {
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (!genParam || constructors.Any(x => !x.GetParameters().Any()))
            {
                return Activator.CreateInstance(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null);
            }

            foreach (var constructor in constructors)
            {
                try
                {
                    return constructor.Invoke(constructor.GetParameters().Select(x => CreateInstance(x.ParameterType, true)).ToArray());
                }
                catch { }
            }

            return null;
        }
    }
}
