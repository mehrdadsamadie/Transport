using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class WayRepository
    {
        TransportContext Context = new TransportContext();
        public List<Way> All(string searchString, int? pathId)
        {
            return Context.Waies.Where(x => pathId == null || x.PathId == pathId).ToList().Where(x => (string.IsNullOrEmpty(searchString) || x.Name.ToLower().Contains(searchString.ToLower()))).OrderBy(x => x.Name)
                .ToList();
        }
        public Way GetById(int id)
        {
            return Context.Waies.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Way model)
        {
            Context.Waies.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Way model)
        {
            Context.Waies.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void Delete(int id)
        {
            var _way = Context.Waies.FirstOrDefault(x => x.Id == id);
            Context.Waies.Remove(_way);
            Context.SaveChanges();
        }
    }
}
