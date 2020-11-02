using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ContractRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<Contract> GetByDriverId(int driverId)
        {
            return Context.Contracts.Where(x => x.DriverId == driverId);
        }
        public Contract GetById(int id)
        {
            return Context.Contracts.FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<Contract> GetAll(DateTime? startDate, DateTime? endDate, bool? isDelete, bool? isActive, bool? IsValid, bool? isActiveNow)
        {
            return Context.Contracts.Where(x =>
            (x.IsDelete || isDelete == null)
            && (x.IsActive || isActive == null)
            && (x.IsValid || IsValid == null)
            && (x.StartDate >= startDate || startDate == null)
            && (x.EndDate <=endDate || endDate == null)
            && (x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now) || isActiveNow == null);
        }
        public int Add(Contract model)
        {
            Context.Contracts.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Contract model)
        {
            Context.Contracts.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
