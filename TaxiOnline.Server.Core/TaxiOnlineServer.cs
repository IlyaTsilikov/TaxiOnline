﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiOnline.Server.Core.Objects;
using TaxiOnline.ServerInfrastructure;
using TaxiOnline.ServerInfrastructure.EntitiesInterfaces;

namespace TaxiOnline.Server.Core
{
    public class TaxiOnlineServer : ITaxiOnlineServer
    {
        private readonly Lazy<ITaxiOnlineMobileService> _mobileService;
        private readonly Lazy<ITaxiOnlineStorage> _storage;
        private Func<ITaxiOnlineServer, ITaxiOnlineMobileService> _mobileServiceInitDelegate;
        private Func<ITaxiOnlineServer, ITaxiOnlineStorage> _storageInitDelegate;

        public ITaxiOnlineMobileService MobileService
        {
            get { return _mobileService.Value; }
        }

        public ITaxiOnlineStorage Storage
        {
            get { return _storage.Value; }
        }

        public TaxiOnlineServer()
        {
            _mobileService = new Lazy<ITaxiOnlineMobileService>(() => _mobileServiceInitDelegate(this), true);
            _storage = new Lazy<ITaxiOnlineStorage>(() => _storageInitDelegate(this), true);
        }

        public void InitMobileService(Func<ITaxiOnlineServer, ITaxiOnlineMobileService> mobileServiceInitDelegate)
        {
            _mobileServiceInitDelegate = mobileServiceInitDelegate;
        }

        public void InitStorage(Func<ITaxiOnlineServer, ITaxiOnlineStorage> storageInitDelegate)
        {
            _storageInitDelegate = storageInitDelegate;
        }

        public IEnumerable<ICityInfo> EnumerateCities(string userCultureName)
        {
            return _storage.Value.EnumerateCities(userCultureName);
        }


        public ICityInfo CreateCityInfo(Guid id)
        {
            return new CityInfo(id);
        }
    }
}