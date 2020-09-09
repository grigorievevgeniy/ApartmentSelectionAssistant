using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public abstract class EntityBase
    {
        public virtual Guid UID { get; set; }
    }
}
