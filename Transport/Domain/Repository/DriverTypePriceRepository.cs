using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DriverTypePriceRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<DriverTypePrice> All(int? driverTypeId, bool? isDelete, bool? isLock)
        {
            return Context.DriverTypePrices.Where(x => (driverTypeId == null || x.DriverTypeId == driverTypeId) && (isDelete == null || x.IsDelete == isDelete) && (isLock == null || x.IsLock == isLock));
        }
        public DriverTypePrice GetById(int id)
        {
            return Context.DriverTypePrices.FirstOrDefault(x => x.Id == id);
        }
        public int Add(DriverTypePrice model)
        {
            Context.DriverTypePrices.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(DriverTypePrice model)
        {
            Context.DriverTypePrices.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public bool HasOverlap(int id, DateTime StartDate, DateTime EndDate)
        {
            bool overlap = false;
            if (Context.DriverTypePrices.Any(x => (id == 0 || x.Id != id) && (
            (x.StartDate >= StartDate && x.StartDate <= EndDate) ||
            (x.EndDate >= StartDate && x.EndDate <= EndDate) || (x.StartDate <= StartDate && x.EndDate >= EndDate))))
                overlap = true;
            return overlap;
        }
    }
}
