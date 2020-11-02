using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class LicensePlateTypeRepository
    {
        private TransportContext Context = new TransportContext();

        public IQueryable<LicensePlateType> All()
        {
            return Context.LicensePlateTypes;
        }
    }
}
