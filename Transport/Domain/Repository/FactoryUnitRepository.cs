using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class FactoryUnitRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<FactoryUnit> All()
        {
            return Context.FactoryUnits;
        }
        public IQueryable<FactoryUnit> GeByFactoryId(int factoryId)
        {
            return Context.FactoryUnits.Where(x => x.FactoryId == factoryId);
        }
    }
}
