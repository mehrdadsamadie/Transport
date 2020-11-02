using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class RDriverDriverTypeRepository
    {
        TransportContext Context = new TransportContext();
        public RDriverDriverType GetByDriverId(int DriverId, bool? isActvieNow, bool? isActive, bool? isDelete)
        {
            var _now = DateTime.Now;
            return Context.RDriverDriverTypes.FirstOrDefault(x =>
            x.DriverId == DriverId
            && (x.StartDate <= _now && x.EndDate >= _now || isActvieNow == null)
            && (x.IsActive == isActive || isActive == null)
            && (x.IsDelete == isDelete || isDelete == null));
        }
        public RDriverDriverType GetById(int id)
        {
            return Context.RDriverDriverTypes.FirstOrDefault(x => x.Id == id);
        }
        public int Add(RDriverDriverType model)
        {
            Context.RDriverDriverTypes.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(RDriverDriverType model)
        {
            Context.RDriverDriverTypes.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
