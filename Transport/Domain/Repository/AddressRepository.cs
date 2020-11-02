using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class AddressRepository
    {
        TransportContext Context = new TransportContext();
        public List<AddressJoin> AutoAddress(string searchString, int? _personnelId, bool isBegin = true)
        {
            var temp = new List<Address>();
            if (isBegin)
            {
                temp = Context.Addresses.Where(x => x.RequestsBeginingAddress.OrderBy(o => o.Id).Take(100).Any(y => y.PersonelId == _personnelId) && x.Address1.ToLower().Contains(searchString)).ToList();

            }
            else
            {
                temp = Context.Addresses.Where(x => x.RequestsDestinationAddress.OrderBy(o => o.Id).Take(100).Any(y => y.PersonelId == _personnelId) && x.Address1.ToLower().Contains(searchString)).ToList();
            }


            var _suggestionAdresses = Context.Suggestions.Where(x => (!string.IsNullOrEmpty(searchString) && (x.Title.ToLower().Contains(searchString) || x.Address.ToLower().Contains(searchString)))).Select(x => new AddressJoin
            {
                Address = x.Address,
                CityName = x.CityName,
                CountryName = x.CountryName,
                EparchyId = x.EparchyId,
                PostalCode = x.PostalCode,
                RegionName = x.RegionName,
                StateId = x.Eparchy.StateId,
                EparchyName=x.Eparchy.Name
            }).ToList();
            _suggestionAdresses = _suggestionAdresses.Union(temp.Select(x => new AddressJoin
            {
                Address = x.Address1,
                CityName = x.CityName,
                CountryName = x.CountryName,
                EparchyId = x.EparchyId,
                RegionName = x.RegionName,
                StateId = x.Eparchy.StateId,
                PostalCode = x.PostalCode,
                EparchyName = x.Eparchy.Name
            })).Distinct().ToList();
            return _suggestionAdresses;
        }
        public Address GetById(int id)
        {
            return Context.Addresses.FirstOrDefault(x => x.Id == id);
        }
        public int Add(Address model)
        {
            Context.Addresses.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Address model)
        {
            Context.Addresses.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
