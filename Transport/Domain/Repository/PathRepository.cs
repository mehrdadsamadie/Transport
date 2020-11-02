using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Repository
{
    public class PathRepository
    {
        TransportContext Context = new TransportContext();
        public List<Path> All(string searchString, bool? isDelete)
        {
            return Context.Paths.Where(x => (x.IsDelete == isDelete || isDelete == null)).ToList().Where(x => (string.IsNullOrEmpty(searchString) || x.Name.ToLower().Contains(searchString.ToLower()))).OrderBy(x=>x.Name)
                .ToList();
        }
        public Path GetById(int id)
        {
            return Context.Paths.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Path model)
        {
            Context.Paths.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Path model)
        {
            Context.Paths.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }

    }
}
