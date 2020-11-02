using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Models
{
    public class PersonelView
    {
        public string FullName { get; internal set; }
        public bool Gender { get; internal set; }
        public string Mobile { get; internal set; }
        public string PersonnelCode { get; internal set; }
    }
}