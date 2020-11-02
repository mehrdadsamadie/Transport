using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Repository
{
    public class DistanceRepository
    {
        TransportContext Context = new TransportContext();
        public List<Distance> All(bool? isDelete)
        {
            return Context.Distances.Where(x => (x.IsDelete == isDelete || isDelete == null)).ToList();
        }
        public Distance GetById(int id)
        {
            return Context.Distances.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Distance model)
        {
            Context.Distances.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Distance model)
        {
            Context.Distances.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
