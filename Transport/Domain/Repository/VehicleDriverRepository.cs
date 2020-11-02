using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class VehicleDriverRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<VehicleDriver> All(DateTime? startDate, DateTime? endDate, bool? isActive, bool? isDelete, bool? isActiveNow)
        {
            return Context.VehicleDrivers.Where(x =>
            (x.IsDelete == isDelete || isDelete == null)
            && (x.IsActive == isActive || isActive == null)
            && ((x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now) || isActiveNow == null)
            && (x.StartDate > startDate || startDate == null)
            && (x.EndDate < endDate || endDate == null)
            );
        }
        public Driver GetDriverOfVehicle(int vehicleId)
        {
            var _now = DateTime.Now;
            var _vehicledriver= Context.VehicleDrivers.Include(x=>x.Driver.Personnel).FirstOrDefault(x => x.VehicleId == vehicleId
            && x.IsActive == true
            && x.IsDelete == false
            && (x.StartDate == null || x.StartDate <= _now)
            && (x.EndDate == null || x.EndDate >= DateTime.Now)
            );
            return _vehicledriver.Driver;
        }
        public VehicleDriver GetVehicleDriver(int vehicleId)
        {
            var _now = DateTime.Now;
            var _vehicledriver = Context.VehicleDrivers.Include(x => x.Driver.Personnel).FirstOrDefault(x => x.VehicleId == vehicleId
               && x.IsActive == true
               && x.IsDelete == false
               && (x.StartDate == null || x.StartDate <= _now)
               && (x.EndDate == null || x.EndDate >= DateTime.Now)
            );
            return _vehicledriver;
        }
        public int Add(VehicleDriver model)
        {
            Context.VehicleDrivers.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(VehicleDriver model)
        {
            Context.VehicleDrivers.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
