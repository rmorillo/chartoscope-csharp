using System;

namespace Chartoscope.Common
{
    public interface ILookBehindReader<T>
    {
        T this[int index] { get; }

        T Current { get; }

        T Previous { get; }

        int Count { get; }

        long Sequence { get; }

        int Position { get; }

        int Capacity { get; }
    }
}

