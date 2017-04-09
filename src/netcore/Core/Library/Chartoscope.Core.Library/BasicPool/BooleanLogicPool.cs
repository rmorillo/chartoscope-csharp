using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    /// <summary>
    /// Look behind pool of boolean values
    /// </summary>
    public class BooleanLogicPool : LookBehindPool<bool>
    {
        /// <summary>
        /// Create new instance with pool capacity
        /// </summary>
        /// <param name="capacity">Number of pre-allocated items in a pool</param>
        public BooleanLogicPool(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Writes new boolean value to the pool
        /// </summary>
        /// <param name="value">Boolean value</param>
        public void Write(bool value)
        {
            WriteCopy(value);
        }

        /// <summary>
        /// Pool item initializer
        /// </summary>
        /// <returns>Initialized boolean value</returns>
        protected override bool CreatePoolItem()
        {
            return false;
        }
    }
}
