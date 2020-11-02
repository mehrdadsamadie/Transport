using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class StateRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<State> All()
        {
            return Context.States.OrderBy(x=>x.Name);
        }
    }
}
