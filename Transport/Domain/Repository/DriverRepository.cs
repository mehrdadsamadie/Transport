using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DriverRepository
    {
        TransportContext Context = new TransportContext();
        public List<Driver> GetAll(string searchstr, bool? isActive, bool? isDelete, bool? IsBlack)
        {
            return Context.Drivers.Include(x => x.Personnel).Where(x => (x.IsActive == isActive || isActive == null) && (x.IsDelete == isDelete || isDelete == null) && (x.IsBlack == IsBlack || IsBlack == null)).ToList()
                .Where(x=> string.IsNullOrEmpty(searchstr) || x.Personnel.FullName.ToLower().Contains(searchstr.ToLower())).ToList();
        }
        public Driver GetById(int id)
        {
            return Context.Drivers.FirstOrDefault(x => x.Id==id);
        }

        public Driver GetByName(string DriverName)
        {
            return Context.Drivers.FirstOrDefault(x => x.Personnel.FullName == DriverName);
        }
        public int Add(Driver model)
        {
            Context.Drivers.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Driver model)
        {
            Context.Drivers.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
