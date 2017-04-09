using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class EntityHeader
    {
        public EntityHeader(string codeName, string displayName, string description)
        {
            CodeName = codeName;
            DisplayName = displayName;
            Description = description;

            Fields = new Dictionary<int, EntityField>();
        }
        public string CodeName { get; private set; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }        

        internal Dictionary<int, EntityField> Fields { get; private set; }

        public int TotalByteSize
        {
            get
            {
                int totalByteSize = 0;

                foreach(var field in Fields.Values)
                {
                    totalByteSize += field.Size;
                }

                return totalByteSize;
            }
        }

        public byte[] ToByteArray()
        {
            return new byte[1];
        }
    }
}
