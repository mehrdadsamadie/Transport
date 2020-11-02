using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class PersonnelRepository
    {
        TransportContext Context = new TransportContext();
        public List<Personnel> All(string searchString, bool? isDelete, bool? isArchive,Enums.OrderEnumPersonnels order=Enums.OrderEnumPersonnels.LastName ,bool isAsc=true)
        {
            var _personnels = Context.Personnels.Where(x => (x.IsArchive == isArchive || isArchive == null) && (x.IsDelete == isDelete || isDelete == null))
                .ToList().Where(x=>(string.IsNullOrEmpty(searchString) || x.FullName.ToLower().Contains(searchString.ToLower()))).ToList();
            switch (order)
            {
                case Enums.OrderEnumPersonnels.LastName:
                    {
                        if (isAsc)
                        {
                            _personnels = _personnels.OrderBy(x=>x.LastName).ThenBy(x => x.Name).ToList();
                        }
                        else
                        {
                            _personnels = _personnels.OrderByDescending(x =>x.LastName).ThenByDescending(x => x.Name).ToList();
                        }
                    }
                    break;
                case Enums.OrderEnumPersonnels.PersonnelCode:
                    {
                        if (isAsc)
                        {
                            _personnels = _personnels.OrderBy(x => x.PersonnelCode).ThenBy(x => x.PersonnelCode).ToList();
                        }
                        else
                        {
                            _personnels = _personnels.OrderByDescending(x => x.PersonnelCode).ThenByDescending(x => x.PersonnelCode).ToList();
                        }
                    }
                    break;
            }
            return _personnels;
        }
        public Personnel GetById(int id)
        {
            var _personnel = Context.Personnels.FirstOrDefault(x => x.Id == id);
            return _personnel;
        }
        public Personnel GetByCode(string personnelCode)
        {
            var _personnel = Context.Personnels.FirstOrDefault(x => x.PersonnelCode == personnelCode);
            return _personnel;
        }
        public int Add(Personnel model)
        {
            Context.Personnels.Add(model);
            Context.SaveChanges();
            return model.Id;
        }
        public void Edit(Personnel model)
        {
            Context.Personnels.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
