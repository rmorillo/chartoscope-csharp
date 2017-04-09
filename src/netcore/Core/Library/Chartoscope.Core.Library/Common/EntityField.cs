using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class EntityField
    {
        public EntityField(string codeName, string displayName, string description, Type type, int size)
        {
            CodeName = codeName;            
            DisplayName = displayName;
            Description = description;
            Type = type;
            Size = size;
        }

        public string CodeName { get; private set; }

        public int Sequence { get; private set; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public Type Type { get; private set; }

        public int Size { get; private set; }

        public EntityField Register(EntityHeader header)
        {
            Sequence = header.Fields.Count;
            header.Fields.Add(Sequence, this);

            return this;
        }
    }
}
