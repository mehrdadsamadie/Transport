using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
     public class PersonStatusTypeRepository
    {
        TransportContext Context = new TransportContext();
        public List<PersonStatusType> All()
        {
            var _personStatusTypes = Context.PersonStatusTypes.ToList();
            return _personStatusTypes;
        }
        public PersonStatusType GetById(int id)
        {
            var _personStatusType = Context.PersonStatusTypes.FirstOrDefault(x => x.Id == id);
            return _personStatusType;
        }
    }
}
