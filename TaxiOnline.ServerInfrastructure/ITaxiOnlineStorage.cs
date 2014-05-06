﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiOnline.ServerInfrastructure.EntitiesInterfaces;

namespace TaxiOnline.ServerInfrastructure
{
    public interface ITaxiOnlineStorage
    {
        IEnumerable<ICityInfo> EnumerateCities();
        IEnumerable<ICityInfo> EnumerateCities(string userCultureName);
        IEnumerable<IPedestrianInfo> EnumeratePedestrians(Guid cityId);
        IEnumerable<IDriverInfo> EnumerateDrivers(Guid cityId);
    }
}
