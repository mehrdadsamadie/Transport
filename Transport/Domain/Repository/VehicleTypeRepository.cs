using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class VehicleTypeRepository
    {
        private TransportContext Context = new TransportContext();
        public IQueryable<VehicleType> All()
        {
            return Context.VehicleTypes;
        }
    }
}
