﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxiOnline.ServerInfrastructure.EntitiesInterfaces;

namespace TaxiOnline.Server.Core.Objects
{
    internal class PersonInfo : IPersonInfo
    {
        private readonly Guid _id;
        private bool _isOnline;
        private double _currentLocationLatidude;
        private double _currentLocationLongidude;
        private string _phoneNumber;
        private string _skypeNumber;

        public Guid Id
        {
            get { return _id; }
        }

        public bool IsOnline
        {
            get { return _isOnline; }
            internal set { _isOnline = value; }
        }

        public double CurrentLocationLatidude
        {
            get { return _currentLocationLatidude; }
            set { _currentLocationLatidude = value; }
        }

        public double CurrentLocationLongidude
        {
            get { return _currentLocationLongidude; }
            set { _currentLocationLongidude = value; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public string SkypeNumber
        {
            get { return _skypeNumber; }
            set { _skypeNumber = value; }
        }

        public PersonInfo(Guid id)
        {
            _id = id;
        }


    }
}
