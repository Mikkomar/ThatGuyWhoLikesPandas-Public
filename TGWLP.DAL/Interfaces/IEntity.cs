using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public Guid Creator { get; set; }
        public DateTime Created { get; set; }
        public bool IsNewEntity();
    }
}
