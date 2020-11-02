using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ServiceRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<Service> All(int? driverId, IEnumerable<int> serviceStatusId, DateTime? startDate, DateTime? endDate,TimeSpan? startTime,TimeSpan? endTime, bool? isAcceptTranasportManager, bool? isDelete, bool? isLock = null, Enums.OrderEnumService order = Enums.OrderEnumService.ServiceDate, bool isAsc = true)
        {
            var _now = DateTime.Now;
            var _services = Context.Services.Where(x => (driverId == null || x.DriverId == driverId)
              && (startDate == null || x.Date >= startDate.Value)
              && (endDate == null || x.Date <= endDate.Value)
              && (!serviceStatusId.Any() || serviceStatusId.Contains(x.ServiceStatusId))
              && (isAcceptTranasportManager == null || x.IsAcceptTranasportManager == isAcceptTranasportManager)
              && (isLock == null || x.IsLock == isLock)
              && (startTime == null || x.StartTime >= startTime) 
              && (endTime == null || x.StartTime <= endTime)
              && (isDelete == null || x.IsDelete == isDelete));
            switch (order)
            {
                case Enums.OrderEnumService.ServiceDate:
                    {
                        if (isAsc)
                        {
                            _services = _services.OrderByDescending(x => System.Data.Entity.DbFunctions.DiffDays(_now, x.Date)).ThenByDescending(x => x.StartTime).AsQueryable();
                        }
                        else
                        {
                            _services = _services.OrderBy(x => System.Data.Entity.DbFunctions.DiffDays(_now, x.Date)).OrderBy(x => x.StartTime).AsQueryable();
                        }
                    }
                    break;
                case Enums.OrderEnumService.DriverId:
                    {
                        if (isAsc)
                        {
                            var _servicesdriver = _services.Where(x => x.Driver != null).OrderBy(x => x.Driver.Personnel.LastName).ThenBy(x => x.Driver.Personnel.Name).AsQueryable();
                            var _servicesTaxiCompany = _services.Where(x => x.Driver == null).OrderBy(x => x.TaxiCompany.Name).ThenBy(x => x.TaxiCompany.Name).ThenBy(x => x.DriverName).AsQueryable();
                            _services = _servicesdriver.Union(_servicesTaxiCompany);
                        }
                        else
                        {
                            var _servicesdriver = _services.Where(x => x.Driver != null).OrderByDescending(x => x.Driver.Personnel.LastName).ThenByDescending(x => x.Driver.Personnel.Name).AsQueryable();
                            var _servicesTaxiCompany = _services.Where(x => x.Driver == null).OrderByDescending(x => x.TaxiCompany.Name).ThenByDescending(x => x.TaxiCompany.Name).ThenByDescending(x => x.DriverName).AsQueryable();
                            _services = _servicesdriver.Union(_servicesTaxiCompany);
                        }
                    }
                    break;

            }
            return _services;

        }
        public Service GetById(int id)
        {
            return Context.Services.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Service model)
        {
            Context.Services.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Service model)
        {
            Context.Services.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public IQueryable<Service> GetServiceByAutoId(string searchStr)
        {
            IEnumerable<int> _serviceStatus = new List<int> { (int)Enums.ServiceStatus.NotStart, (int)Enums.ServiceStatus.Servicing };
            return All(null, _serviceStatus, null, null,null,null, null, false, false).Where(x => x.Id.ToString().StartsWith(searchStr));
        }
    }
}
