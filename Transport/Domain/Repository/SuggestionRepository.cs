using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class SuggestionRepository
    {
        TransportContext Context = new TransportContext();
        public IQueryable<Suggestion> GetAll(string searchString, int? eparchyId)
        {
            return Context.Suggestions.Where(x =>
            (eparchyId == null || x.EparchyId == eparchyId.Value) &&
            (string.IsNullOrEmpty(searchString) || x.Title.ToLower().Contains(searchString)));
        }
    }
}
