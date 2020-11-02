using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DelayPriceRepository
    {
        private TransportContext Context = new TransportContext();

        public IQueryable<DelayPrice> All(int? driverTypeId,bool? isDelete)
        {
            return Context.DelayPrices.Where(x => (driverTypeId == null || x.DriverTypeId == driverTypeId) && (isDelete == null || x.IsDelete == isDelete));
        }
        public DelayPrice GetById(int id)
        {
            return Context.DelayPrices.FirstOrDefault(x => x.Id == id);
        }
        public int Add(DelayPrice model)
        {
            Context.DelayPrices.Add(model);
            Context.SaveChanges();
            return model.Id;
        }

        public bool HasOverlap(int id,int drivertypeId, DateTime StartDate, DateTime EndDate)
        {
            bool overlap = false;

            if (Context.DelayPrices.Any(x => (id == 0 || x.Id != id) && (x.DriverTypeId == drivertypeId) && (
            (x.StartDate >= StartDate && x.StartDate <= EndDate) ||
            (x.EndDate >= StartDate && x.EndDate <= EndDate) || (x.StartDate <= StartDate && x.EndDate >= EndDate))))
                overlap = true;
            return overlap;
        }
        public void Edit(DelayPrice model)
        {
            Context.DelayPrices.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
