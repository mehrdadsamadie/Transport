using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class TaxiCompanyRepository
    {
        TransportContext Context = new TransportContext();
        public List<TaxiCompany> All(string searchString, bool? isDelete)
        {
            return Context.TaxiCompanies.Where(x =>(x.IsDelete == isDelete || isDelete == null)).ToList().Where(x => (string.IsNullOrEmpty(searchString) || x.Name.ToLower().Contains(searchString.ToLower()))).ToList();
        }
        public TaxiCompany GetById(int id)
        {
            return Context.TaxiCompanies.FirstOrDefault(x => x.Id == id);
        }
        public int Add(TaxiCompany model)
        {
            Context.TaxiCompanies.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(TaxiCompany model)
        {
            Context.TaxiCompanies.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }

}
