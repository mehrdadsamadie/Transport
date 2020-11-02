using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class OverTimeRepository
    {
        public List<OverTime> All( DateTime? StartDate, DateTime? EndDate, bool? IsLock = null, bool? ShowDelete = false)
        {
            List<OverTime> _overTimes;
            using (TransportContext Context = new TransportContext())
            {
                _overTimes = Context.OverTimes
                   .Where(x => (ShowDelete == true || x.IsDelete == false) &&
                   (!StartDate.HasValue || DbFunctions.TruncateTime(StartDate) <= DbFunctions.TruncateTime(x.StartDate)) &&
                   (!EndDate.HasValue || DbFunctions.TruncateTime(EndDate) >= DbFunctions.TruncateTime(x.EndDate)) &&
                   (IsLock == null || x.IsLock == IsLock)
                   ).ToList();
            }
            return _overTimes;
        }

        public OverTime GetById(int Id)
        {
            OverTime _overTime;
            using (TransportContext context = new TransportContext())
            {
                _overTime = context.OverTimes.FirstOrDefault(x => x.Id == Id);
            }
            return _overTime;
        }

        public void Add(OverTime model)
        {
            using (TransportContext context = new TransportContext())
            {
                context.OverTimes.Add(model);
                context.SaveChanges();
            }
        }

        public void Edit(OverTime model)
        {
            using (TransportContext Context = new TransportContext())
            {
                Context.OverTimes.Attach(model);
                Context.Entry(model).State = EntityState.Modified;
                Context.SaveChanges();
            }
        }

        public bool HasOverlap(int id ,DateTime StartDate , DateTime EndDate)
        {
            bool overlap = false;
            using (TransportContext context = new TransportContext())
            {
                if (context.OverTimes.Any(x => (id == 0 || x.Id != id) &&  (
                (x.StartDate >= StartDate && x.StartDate <= EndDate) ||
                (x.EndDate >= StartDate && x.EndDate <= EndDate) || (x.StartDate <= StartDate && x.EndDate >= EndDate))))
                    overlap = true;
            }
            return overlap;
        }
    }
}
