using System;

namespace Chartoscope.Core
{
    public interface ILookBehindReader<T>
    {
        T this[int index] { get; }

        T Current { get; }
		
		T Previous { get; }

        int Length { get; }
		
		long Sequence { get; }

		int Position { get; }
    }
}

