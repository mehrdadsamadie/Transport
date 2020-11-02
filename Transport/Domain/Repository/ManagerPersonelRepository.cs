using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ManagerPersonelRepository
    {
        TransportContext context = new TransportContext();
        public IQueryable<ManagerPersonel> GetManagerOfPersonnel(int personnelId)
        {
            return context.ManagerPersonels.Where(x => x.PersonelId == personnelId);
        }
        public List<ManagerPersonel> GetPersonnelOfManager(int managerId, string searchString, bool? isDelete, bool? isArchive)
        {
            return context.ManagerPersonels.Where(x => x.ManagerPersonelId == managerId &&
            (x.Personnel.IsArchive == isArchive || isArchive == null) &&
            (x.Personnel.IsDelete == isDelete || isDelete == null)).ToList()
            .Where(x => (string.IsNullOrEmpty(searchString) || x.Personnel.FullName.ToLower().Contains(searchString.ToLower()))).ToList();

        }
    }
}
