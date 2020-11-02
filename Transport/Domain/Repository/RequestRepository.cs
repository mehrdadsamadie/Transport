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
    public class RequestRepository
    {
        TransportContext Context = new TransportContext();
        //public List<Request> All(int CurrentPersonelID, DateTime? startDate, DateTime? endDate, bool? isDelete, bool? isAcceptTranasport, bool? isAcceptManager, int? PersonelIdFilter = null, int? FactoryUnitId = null)
        //{
        //    var _now = DateTime.Now;
        //    return Context.Requests.Include(x => x.Personnel).Where(x =>
        //      (isAcceptTranasport == null || x.IsAcceptTranasport == isAcceptTranasport) &&
        //      (isDelete == null || x.IsDelete == isDelete) &&
        //      (x.PersonelId == CurrentPersonelID || isAcceptManager == null || x.IsAcceptManager == isAcceptManager) &&
        //      (PersonelIdFilter == null || x.PersonelId == PersonelIdFilter || (FactoryUnitId != null && (x.Personnel.FactoryUnitId == FactoryUnitId)))
        //      && (startDate == null || x.ServiceDate >= startDate)
        //      && (endDate == null || x.ServiceDate <= endDate)
        //      )
        //    .OrderByDescending(x => EntityFunctions.DiffDays(_now, x.ServiceDate)).ThenByDescending(x => x.StartTime).ToList()
        //    //.ToList().OrderBy(x => (_now - x.ServiceDate.Value).TotalDays).ToList();
        //    ;
        //}
        public IQueryable<Request> AlltempIntenal(int CurrentPersonelID, DateTime? startDate, DateTime? endDate, TimeSpan? startTime, TimeSpan? endTime, bool? isDelete, bool? isCancel, IEnumerable<int> acceptTranasport, IEnumerable<int> acceptManager, int? personelIdFilter = null, int? factoryUnitId = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.ServiceDate, bool isAsc = true)
        {
            var _now = DateTime.Now;
            var _requests = Context.Requests.Include(x => x.Personnel).Where(x =>
               (!acceptTranasport.Any() || acceptTranasport.Contains(x.IsAcceptTranasport)) &&
               (!acceptManager.Any() || acceptManager.Contains(x.IsAcceptManager)) &&
               (isDelete == null || x.IsDelete == isDelete) &&
               (isCancel == null || x.IsCanceled == isCancel) &&
               (startDate == null || x.ServiceDate >= startDate) &&
               (endDate == null || x.ServiceDate <= endDate) &&
               (startTime == null || x.StartTime >= startTime) &&
               (endTime == null || x.StartTime <= endTime)
              ).AsQueryable();
            switch (order)
            {
                case Enums.OrderEnumRequest.ServiceDate:
                    {
                        if (isAsc)
                        {
                            _requests = _requests.OrderByDescending(x => System.Data.Entity.DbFunctions.DiffDays(_now, x.ServiceDate)).ThenByDescending(x => x.StartTime).AsQueryable();
                        }
                        else
                        {
                            _requests = _requests.OrderBy(x => System.Data.Entity.DbFunctions.DiffDays(_now, x.ServiceDate)).OrderBy(x => x.StartTime).AsQueryable();
                        }
                    }
                    break;
                case Enums.OrderEnumRequest.RequestDate:
                    {
                        if (isAsc)
                        {
                            _requests = _requests.OrderByDescending(x => x.RequestDate).AsQueryable();
                        }
                        else
                        {
                            _requests = _requests.OrderBy(x => x.RequestDate).AsQueryable();
                        }
                    }
                    break;
                case Enums.OrderEnumRequest.PersonelName:
                    {
                        if (isAsc)
                        {
                            _requests = _requests.OrderBy(x => x.Personnel.LastName).ThenBy(x => x.Personnel.Name).AsQueryable();
                        }
                        else
                        {
                            _requests = _requests.OrderByDescending(x => x.Personnel.LastName).ThenByDescending(x => x.Personnel.Name).AsQueryable();

                        }
                    }
                    break;
            }
            return _requests.AsQueryable();
        }
        public List<Request> ManagerRequests(int managerPersonelId, DateTime? startDate, DateTime? endDate, TimeSpan? startTime, TimeSpan? endTime, bool? isDelete, bool? isCancel, IEnumerable<int> acceptTranasport, IEnumerable<int> acceptManager, int? personelIdFilter = null, int? factoryUnitId = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.RequestDate, bool isAsc = true)
        {
            return AlltempIntenal(managerPersonelId, startDate, endDate, startTime, endTime, isDelete, isCancel, acceptTranasport, acceptManager,
                personelIdFilter, factoryUnitId, order, isAsc).Where(x => (personelIdFilter == null && Context.ManagerPersonels.Any(y => y.PersonelId == x.PersonelId && y.ManagerPersonelId == managerPersonelId))
                  || (personelIdFilter != null && x.PersonelId == personelIdFilter && Context.ManagerPersonels.Any(y => y.PersonelId == personelIdFilter && y.ManagerPersonelId == managerPersonelId))
                ).ToList();
        }
        public List<Request> TrnasportRequests(int transportPersonelId, DateTime? startDate, DateTime? endDate, TimeSpan? startTime, TimeSpan? endTime, bool? isDelete, bool? isCancel, IEnumerable<int> acceptTranasport, IEnumerable<int> acceptManager, int? personelIdFilter = null, int? factoryUnitId = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.RequestDate, bool isAsc = true)
        {
            return AlltempIntenal(transportPersonelId, startDate, endDate, startTime, endTime, isDelete, isCancel, acceptTranasport, acceptManager,
                 personelIdFilter, factoryUnitId, order, isAsc).Where(x => (personelIdFilter == null || x.PersonelId == personelIdFilter)
                 ).ToList();

        }
        public List<Request> UserRequests(int personelId, DateTime? startDate, DateTime? endDate, TimeSpan? startTime, TimeSpan? endTime, bool? isDelete, bool? isCancel, IEnumerable<int> acceptTranasport, IEnumerable<int> acceptManager, int? personelIdFilter = null, int? factoryUnitId = null, Enums.OrderEnumRequest order = Enums.OrderEnumRequest.RequestDate, bool isAsc = true)
        {
            return AlltempIntenal(personelId, startDate, endDate, startTime, endTime, isDelete, isCancel, acceptTranasport, acceptManager,
                personelIdFilter, factoryUnitId, order, isAsc).Where(x => x.PersonelId == personelId)
                .ToList();
        }
        public Request GetByServiceId(int serviceId)
        {
            return Context.Requests.FirstOrDefault(x => x.ServiceId == serviceId);
        }
        public IQueryable<Request> GetRequestsByServiceId(int serviceId)
        {
            return Context.Requests.Where(x => x.ServiceId == serviceId);
        }
        public Request GetById(int id)
        {
            return Context.Requests.Include(x => x.BeginingAddress).Include(x => x.DestinationAddress).Include(x => x.Personnel).FirstOrDefault(x => x.Id == id);
        }
        public int Add(Request model)
        {
            Context.Requests.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Request model)
        {
            Context.Requests.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
