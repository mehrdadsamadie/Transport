using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DistancePriceRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<DistancePrice> All(int? distanceId, int? driverTypeId)
        {
            return Context.DistancePrices.Where(x => (distanceId == null || x.DistanseId == distanceId)
            && (driverTypeId == null || x.DriverTypeId == driverTypeId)
            );
        }
        public DistancePrice GetById(int id)
        {
            return Context.DistancePrices.FirstOrDefault(x => x.Id == id);
        }
        public int Add(DistancePrice model)
        {
            Context.DistancePrices.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public bool HasOverlap(int id, int distanceId, int drivertypeId, DateTime StartDate, DateTime EndDate)
        {
            bool overlap = false;

            if (Context.DistancePrices.Any(x => (id == 0 || x.Id != id) && (x.DriverTypeId == drivertypeId) && (x.DistanseId == distanceId) && (
            (x.StartTime >= StartDate && x.StartTime <= EndDate) ||
            (x.EndTime >= StartDate && x.EndTime <= EndDate) || (x.StartTime <= StartDate && x.EndTime >= EndDate))))
                overlap = true;
            return overlap;
        }

        public void Edit(DistancePrice model)
        {
            Context.DistancePrices.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();

        }
    }
}
