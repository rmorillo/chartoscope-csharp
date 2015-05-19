using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class SignalDelegates
    {
        public delegate void EntrySignalHandler(ISignal source, long sequence, PositionMode position);
        public delegate void ExitSignalHandler(ISignal source, long sequence);
        public delegate void RegistrationHandler(object instance);
    }
}
