using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DailyDayRepository
    {
        TransportContext Context = new TransportContext();
        public List<DailyDay> All()
        {
            var _dailyDays = Context.DailyDays.ToList();
            return _dailyDays;
        }
        public DailyDay GetById(int id)
        {
            var _dailyday = Context.DailyDays.FirstOrDefault(x => x.Id == id);
            return _dailyday;
        }

        public void Add(DailyDay model)
        {
            Context.DailyDays.Add(model);
            Context.SaveChanges();
        }
        public void Edit(DailyDay model)
        {
            Context.DailyDays.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
