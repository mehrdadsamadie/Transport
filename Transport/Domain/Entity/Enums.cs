using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    [NotMapped]
    public class Enums
    {
        public enum OrderEnumRequest
        {
            ServiceDate = 1,
            RequestDate = 2,
            PersonelName = 3,
        };
        public enum OrderEnumService
        {
            ServiceDate = 1,
            DriverId = 2,
        };
        public enum OrderEnumPersonnels
        {
            LastName=1,
            PersonnelCode=2,
        };

        public enum ServiceStatus
        {
            NotStart=1,
            Servicing=2,
            Finished=3
        };

        public enum IsAcceptManager
        {
            NotChecked = 0,
            NotAccepted = 1,
            Accepted = 2,
        };
        public enum IsAcceptTransport
        {
            NotChecked = 0,
            NotAccepted = 1,
            Accepted = 2,
        }
        public enum ServiceType
        {
            Passanger=1,
            Services=2
        }
    }
}
