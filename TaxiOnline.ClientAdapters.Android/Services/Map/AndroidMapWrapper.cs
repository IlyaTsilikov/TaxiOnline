﻿using Android.Content;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxiOnline.ClientInfrastructure.Data;
using TaxiOnline.ClientServicesAdapter.Map;
using TaxiOnline.MapEngine.Android.Common;
using TaxiOnline.Toolkit.Patterns;

namespace TaxiOnline.ClientAdapters.Android.Services.Map
{
    public class AndroidMapWrapper : MapWrapperBase
    {
        //private class MapViewWrapper : DisposableObject
        //{
        //    private readonly MapViewSurface _surface;
        //    private readonly MapView _mapView;
        //    private readonly ViewGroup _viewGroup;

        //    public MapView MapView
        //    {
        //        get { return _mapView; }
        //    }

        //    public MapViewWrapper(Context context, ViewGroup viewGroup)
        //    {
        //        _surface = new MapViewSurface(context);
        //        _mapView = new MapView(context, _surface);
        //        _viewGroup = viewGroup;
        //    }

        //    protected override void DisposeManagedResources()
        //    {
        //        base.DisposeManagedResources();
        //        _viewGroup.RemoveView(_mapView);
        //    }

        //    protected override void DisposeUnmanagedResources()
        //    {
        //        base.DisposeUnmanagedResources();
        //        _mapView.Dispose();
        //        _surface.Dispose();
        //    }
        //}

        //private MapViewWrapper _mapViewWrapper;

        //public IDisposable VisualizeMap(Context context, ViewGroup viewGroup)
        //{
        //    _mapViewWrapper = new MapViewWrapper(context, viewGroup);
        //    _mapViewWrapper.MapView.Map = _map;
        //    SetMapCenter(_mapCenter);
        //    SetMapZoom(_mapZoom);
        //    _mapViewWrapper.MapView.MapTilt = 0;
        //    _mapViewWrapper.MapView.MapAllowTilt = false;
        //    viewGroup.AddView(_mapViewWrapper.MapView);
        //    return _mapViewWrapper;
        //}

        //protected override void SetMapCenter(MapPoint value)
        //{
        //    if (_mapViewWrapper != null)
        //        _mapViewWrapper.MapView.MapCenter = new OsmSharp.Math.Geo.GeoCoordinate(value.Latitude, value.Longitude);
        //}

        //protected override void SetMapZoom(double value)
        //{
        //    if (_mapViewWrapper != null)
        //        _mapViewWrapper.MapView.MapZoom = (float)value;
        //}

        //protected override ClientInfrastructure.Data.MapPoint GetMapCenter()
        //{
        //    return _mapViewWrapper == null ? new MapPoint() : new MapPoint(_mapViewWrapper.MapView.MapCenter.Latitude, _mapViewWrapper.MapView.MapCenter.Longitude);
        //}

        //protected override double GetMapZoom()
        //{
        //    return _mapViewWrapper == null ? 1.0 : _mapViewWrapper.MapView.MapZoom;
        //}
        public AndroidMapWrapper()
            : base()
        {

        }

        public void HookMap(MapEngine.Android.Views.MapView mapView)
        {
            mapView.Map = (AndroidMap)_map;
        }

        protected override TaxiOnline.MapEngine.Common.MapBase CreateMap()
        {
            return new AndroidMap()
            {
                TileSize = new MapEngine.Composing.BitmapSize(256, 256)
            };
        }
    }
}
