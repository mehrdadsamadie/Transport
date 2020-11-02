using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DriverHistoryRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<DriverHistory> GetAll(int? driverId, bool? isDelete)
        {
            return Context.DriverHistories.Where(x => (x.DriverId == driverId || driverId == null) && (x.IsDelete==isDelete||isDelete==null));
        }
        public DriverHistory GetById(int id)
        {
            return Context.DriverHistories.FirstOrDefault(x => x.Id == id);
        }
        public int Add(DriverHistory model)
        {
            Context.DriverHistories.Add(model);
            Context.SaveChanges();
            return model.Id;
        } 
        public void Edit (DriverHistory model)
        {
            Context.DriverHistories.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
