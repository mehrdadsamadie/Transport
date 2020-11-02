using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class DailyTimeRepository
    {
        TransportContext Context = new TransportContext();
        public List<DailyTime> All(DateTime? StartDate, DateTime? EndDate, bool ShowDeleted = false)
        {
            var _dailyTimes = Context.DailyTimes.Where(x => (ShowDeleted == true || x.IsDelete == false) &&
            (StartDate == null || x.StartDate >= StartDate) && (EndDate == null || x.EndDate <= EndDate)).ToList();
            return _dailyTimes;
        }
        public DailyTime GetById(int id)
        {
            var _dailyTime = Context.DailyTimes.FirstOrDefault(x => x.Id == id);
            return _dailyTime;
        }

        public void Add(DailyTime model)
        {
            Context.DailyTimes.Add(model);
            Context.SaveChanges();
        }
        public void Edit(DailyTime model)
        {
            Context.DailyTimes.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public DailyTime GetByDate(DateTime date)
        {
            var _dayofweek = (int)date.DayOfWeek;
            var _dailytime = Context.DailyTimes.FirstOrDefault(x => x.StartDate <= date && x.EndDate >= date && x.WeekenDay == _dayofweek);
            return _dailytime;
        }

        public bool HasOverlap(int id, int weekenDay, DateTime startDate, DateTime endDate)
        {
            bool overlap = false;

            if (Context.DailyTimes.Any(x => (id == 0 || x.Id != id) && (x.WeekenDay == weekenDay) && (x.IsDelete == false) && (
            (x.StartDate >= startDate && x.StartDate <= endDate) ||
            (x.EndDate >= startDate && x.EndDate <= endDate) || (x.StartDate <= startDate && x.EndDate >= endDate))))
                overlap = true;
            return overlap;
        }
    }
}
