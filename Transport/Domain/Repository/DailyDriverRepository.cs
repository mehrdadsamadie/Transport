using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DailyDriverRepository
    {
        
        public List<DailyDriver> All(DateTime? date, int? DriverTypeId, int? StatusTypeId, bool ShowDeletes = false)
        {
            List<DailyDriver> _dailyDrivers;
            using (TransportContext Context = new TransportContext())
            {
                _dailyDrivers = Context.DailyDrivers
                    .Include(y => y.Driver.Personnel).Include(x => x.DriverType).Include(x => x.PersonStatusType).Where(x =>
                 ((!date.HasValue) || DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(date))  &&
               (DriverTypeId == null || x.DriverTypeId == DriverTypeId) &&
               (StatusTypeId == null || x.StatusTypeId == StatusTypeId) &&
               (ShowDeletes == true || x.IsDelete == false)).ToList();
            }
            return _dailyDrivers;
        }
        public DailyDriver GetById(int id)
        {
            DailyDriver _dailyDriver;
            using (TransportContext Context = new TransportContext())
            {
                _dailyDriver = Context.DailyDrivers.Include(y => y.Driver.Personnel).Include(x => x.DriverType).Include(x => x.PersonStatusType).FirstOrDefault(x => x.Id == id);
            }
            return _dailyDriver;
        }
        public void Add(DailyDriver model)
        {
            using (TransportContext Context = new TransportContext())
            {
                Context.DailyDrivers.Add(model);
                Context.SaveChanges();
            }
        }
        public void Edit(DailyDriver model)
        {
            using (TransportContext Context = new TransportContext())
            {
                Context.DailyDrivers.Attach(model);
                Context.Entry(model).State = EntityState.Modified;
                Context.SaveChanges();
            }
        }
    }
}
