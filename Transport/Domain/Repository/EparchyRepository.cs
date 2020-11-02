using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
   public class EparchyRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<Eparchy> All()
        {
            return Context.Eparchies.OrderBy(x=>x.Name);
        }
        public IQueryable<Eparchy> GetByStateId(int stateId)
        {
            return Context.Eparchies.Where(x => x.StateId == stateId).OrderBy(x=>x.Name);
        }
    }
}
