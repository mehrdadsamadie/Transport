using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DriverTypeRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<DriverType> All()
        {
            return Context.DriverTypes;
        }
    }
}
