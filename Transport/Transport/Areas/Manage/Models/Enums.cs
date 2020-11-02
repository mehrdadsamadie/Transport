using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Areas.Manage.Models
{
    enum DriverTypes
    {
        disposal = 1,
        Path = 2
    }
    enum PersonStatusTypes
    {
        Present = 1,
        Absent = 2,
        FaultyEnterance = 3,
        FaultyExit = 4
    }
    enum VehicleGroupView
    {
        عادی = 1,
        تشریفاتی = 2,
        عمومی = 3,
        اختصاصی = 4,
    }
    enum PlateType
    {
        سفید=1,
        زرد=2,
        قرمز=3,
        سایر=4
    }
}