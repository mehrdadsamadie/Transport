using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class VehicleRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<Vehicle> GetAll(int? driverId,bool? isActive,bool? isDelete)
        {
            return Context.Vehicles.Where(x => (x.IsActive == isActive || isActive == null) 
            && (x.IsDelete == isDelete || isDelete == null));
        }

        public Vehicle GetById(int id)
        {
            return Context.Vehicles.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Vehicle model)
        {
            Context.Vehicles.Add(model);
            Context.SaveChanges();
            return model.Id;
        }

        public void Edit(Vehicle model)
        {
            Context.Vehicles.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
