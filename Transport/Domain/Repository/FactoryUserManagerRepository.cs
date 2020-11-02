using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class FactoryUserManagerRepository
    {
        TransportContext Context = new TransportContext();
        public FactoryUserManager GetByPersonnelId(int id)
        {
            return Context.FactoryUserManagers.FirstOrDefault(x => x.PersonnelId == id && x.IsDelete == false);
        }
        public int Add(FactoryUserManager model)
        {
            Context.FactoryUserManagers.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(FactoryUserManager model)
        {
            Context.FactoryUserManagers.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public List<FactoryUserManager> All(string searchString, int? factoryUnitId,bool? isDelete)
        {
            return Context.FactoryUserManagers.Include(x=>x.FactoryUnit.Factory).Where(x => (factoryUnitId == null || x.FactoryUnitId == factoryUnitId) &&(isDelete==null || x.IsDelete==isDelete)).ToList().Where(x=>(string.IsNullOrEmpty(searchString) || x.Personnel.FullName.ToLower().Contains(searchString.ToLower()))).ToList();
        }
        public FactoryUserManager GetById(int id)
        {
            return Context.FactoryUserManagers.FirstOrDefault(x => x.Id == id);
        }

    }
}
