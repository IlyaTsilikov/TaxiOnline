﻿using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxiOnline.ClientInfrastructure.Services;

namespace TaxiOnline.ClientInfrastructure.Android.Services
{
    public interface IAndroidSettingsService : ISettingsService
    {
        ISharedPreferences Preferences { get; }
    }
}
