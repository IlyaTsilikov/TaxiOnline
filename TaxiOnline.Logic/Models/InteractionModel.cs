﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TaxiOnline.Logic.Common;
using TaxiOnline.Logic.Helpers;
using TaxiOnline.Toolkit.Events;
using TaxiOnline.Toolkit.Threading.CollectionsDecorators;

namespace TaxiOnline.Logic.Models
{
    public class InteractionModel
    {
        private ProfileModel _currentProfile;
        private readonly Logic.InteractionLogic _logic;
        private readonly SimpleCollectionLoadDecorator<CityModel> _cities;
        private readonly MapModel _map;
        private CityModel _currentCity;

        public CityModel CurrentCity
        {
            get { return _currentCity; }
            set
            {
                if (_currentCity != value)
                {
                    _currentCity = value;
                    OnCurrentCityChanged();
                }
            }
        }

        public IEnumerable<CityModel> Cities
        {
            get { return _cities.Items; }
        }

        public MapModel Map
        {
            get { return _map; }
        }

        public ProfileModel CurrentProfile
        {
            get { return _currentProfile; }
            internal set
            {
                if (_currentProfile != value)
                {
                    _currentProfile = value;
                    OnCurrentProfileChanged();
                }
            }
        }

        public event EventHandler CurrentCityChanged;

        public event EventHandler CitiesChanged
        {
            add { _cities.ItemsChanged += value; }
            remove { _cities.ItemsChanged -= value; }
        }

        public event EventHandler CurrentProfileChanged;

        public event ActionResultEventHandler EnumrateCitiesFailed;

        internal Func<ActionResult<IEnumerable<Logic.CityLogic>>> EnumerateCitiesDelegate;

        public InteractionModel(AdaptersExtender adaptersExtender)
        {
            _logic = new Logic.InteractionLogic(this, adaptersExtender);
            _cities = new SimpleCollectionLoadDecorator<CityModel>(EnumerateCities);
        }

        public void BeginLoadCities()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => _cities.FillItemsList());
        }

        public void NotifyEnumrateCitiesFailed(ActionResult errorResult)
        {
            OnEnumrateCitiesFailed(errorResult);
        }

        private ActionResult<IEnumerable<CityModel>> EnumerateCities()
        {
            return UpdateHelper.EnumerateModels(EnumerateCitiesDelegate, l => l.Model);
        }

        protected virtual void OnCurrentCityChanged()
        {
            EventHandler handler = CurrentCityChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnCurrentProfileChanged()
        {
            EventHandler handler = CurrentProfileChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnEnumrateCitiesFailed(ActionResult errorResult)
        {
            ActionResultEventHandler handler = EnumrateCitiesFailed;
            if (handler != null)
                handler(this, new ActionResultEventArgs(errorResult));
        }
    }
}
