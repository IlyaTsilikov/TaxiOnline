﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaxiOnline.Logic.Models;
using TaxiOnline.Toolkit.Collections.Helpers;
using TaxiOnline.Toolkit.Events;
using TaxiOnline.Android.Helpers;
using TaxiOnline.Android.Activities;
using TaxiOnline.ClientInfrastructure.Data;
using TaxiOnline.Android.Decorators;

namespace TaxiOnline.Android.Adapters
{
    public class PedestrianProfileAdapter : BaseAdapter
    {
        private readonly Activity _context;
        private readonly PedestrianProfileModel _model;
        private List<DriverModel> _items;
        private ViewsCacheDecorator<DriverModel> _viewCache;
        private Lazy<View> _selfItemView;
        private PopupWindow _driverInfoPopup;

        public PedestrianProfileAdapter(Activity context, PedestrianProfileModel model)
        {
            _context = context;
            _model = model;
            _viewCache = new ViewsCacheDecorator<DriverModel>(() => _context.LayoutInflater.Inflate(Resource.Layout.DriverInfoLayout, null, false));
            _selfItemView = new Lazy<View>(() => _context.LayoutInflater.Inflate(Resource.Layout.PedestrianSelfInfoLayout, null, false));
            model.CheckedDriverChanged += Model_CheckedDriverChanged;
            model.DriversChanged += Model_DriversChanged;
            model.DriversCollectionChanged += Model_DriversCollectionChanged;
            model.CurrentLocationChanged += Model_CurrentLocationChanged;
            model.Map.MapService.Map.MapCenterChanged += Map_MapCenterChanged;
            model.Map.MapService.Map.MapZoomChanged += Map_MapZoomChanged;
            UpdateDrivers();
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _items.Count + 1; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = position == 0 ? _selfItemView.Value : _viewCache.GetView(_items[position - 1]);
            if (position == 0)
                HookCurrentModelToView(view, parent);
            else
                HookModelToView(view, _items[position - 1], parent);
            return view;
        }

        private void HookCurrentModelToView(View view, ViewGroup upperView)
        {
            view.LayoutParameters = MapHelper.GetLayoutParams(upperView, _model.Map.MapService.Map, _model.CurrentLocation);
        }

        private void HookModelToView(View view, DriverModel driverModel, ViewGroup upperView)
        {
            view.LayoutParameters = MapHelper.GetLayoutParams(upperView, _model.Map.MapService.Map, driverModel.CurrentLocation);
            ImageView driverIconImageView = view.FindViewById<ImageView>(Resource.Id.driverIconImageView);
            driverIconImageView.Hover += (sender, e) => ShowDriverInfoPopupWindow(driverModel, view);
            driverIconImageView.Click += (sender, e) => ShowDriverInfoPopupWindow(driverModel, view);
        }

        private void UpdateDrivers()
        {
            IEnumerable<DriverModel> modelCollection = _model.Drivers;
            _items = modelCollection == null ? new List<DriverModel>() : modelCollection.ToList();
            _viewCache.NotifyFillStarted();
            NotifyDataSetChanged();
            _viewCache.NotifyFillFinished();
        }

        private void ShowDriverInfoPopupWindow(DriverModel driverModel, View briefView)
        {
            if (_driverInfoPopup == null)
            {
                _model.CheckedDriver = driverModel;
                _driverInfoPopup = new PopupWindow(_context.LayoutInflater.Inflate(Resource.Layout.DriverPopupDetailsLayout, null), 200, 300);
                HookModelToDetailsPopupWindow(_driverInfoPopup, driverModel);
                _driverInfoPopup.ShowAsDropDown(briefView, 32, -32);
                _driverInfoPopup.Update();
                Handler closePopupWindowHandler = new Handler();
                closePopupWindowHandler.PostDelayed(() =>
                {
                    if (_driverInfoPopup != null && _model.CheckedDriver == driverModel)
                        CloseDriverInfoPopupWindow();
                }, 20000L);
            }
        }

        private void CloseDriverInfoPopupWindow()
        {
            if (_driverInfoPopup != null)
            {
                _driverInfoPopup.Dismiss();
                _driverInfoPopup.Dispose();
                _driverInfoPopup = null;
                _model.CheckedDriver = null;
            }
        }

        private void HookModelToDetailsPopupWindow(PopupWindow popup, DriverModel driverModel)
        {
            HookModelToDetailsView(popup.ContentView, driverModel);
        }

        private void HookModelToDetailsView(View view, DriverModel driverModel)
        {
            _model.SelectedDriver = driverModel;
            view.Click += (sender, e) =>
            {
                CloseDriverInfoPopupWindow();
                UIHelper.GoResultActivity(_context, typeof(PedestrianProfileRequestActivity), 1);
            };
            Button quickCallToDriverButton = view.FindViewById<Button>(Resource.Id.quickCallToDriverButton);
            quickCallToDriverButton.Click += (sender, e) =>
            {
                if (!_model.CallToDriver(driverModel).IsValid)
                    using (Toast errorToast = Toast.MakeText(Application.Context, Resource.String.PhoneCallError, ToastLength.Short))
                        errorToast.Show();
                CloseDriverInfoPopupWindow();
            };
            TextView driverPopupCarBrandTextView = view.FindViewById<TextView>(Resource.Id.driverPopupCarBrandTextView);
            TextView driverPopupCarColorTextView = view.FindViewById<TextView>(Resource.Id.driverPopupCarColorTextView);
            TextView driverPopupCarNumberTextView = view.FindViewById<TextView>(Resource.Id.driverPopupCarNumberTextView);
            TextView driverPopupPersonNameTextView = view.FindViewById<TextView>(Resource.Id.driverPopupPersonNameTextView);
            TextView driverPopupPhoneNumberTextView = view.FindViewById<TextView>(Resource.Id.driverPopupPhoneNumberTextView);
            TextView driverPopupSkypeNumberTextView = view.FindViewById<TextView>(Resource.Id.driverPopupSkypeNumberTextView);
            driverPopupPersonNameTextView.Text = driverModel.PersonName;
            driverPopupCarColorTextView.Text = driverModel.CarColor;
            driverPopupCarBrandTextView.Text = driverModel.CarBrand;
            driverPopupCarNumberTextView.Text = driverModel.CarNumber;
            driverPopupPhoneNumberTextView.Text = driverModel.PhoneNumber;
            driverPopupSkypeNumberTextView.Text = driverModel.SkypeNumber;
            TextView driverAgreesTextView = view.FindViewById<TextView>(Resource.Id.driverAgreesTextView);
            if (driverModel.HasAcceptedRequest)
                driverAgreesTextView.Visibility = ViewStates.Visible;
            driverModel.HasAcceptedRequestChanged += (sender, e) => driverAgreesTextView.Visibility = driverModel.HasAcceptedRequest ? ViewStates.Visible : ViewStates.Gone;
        }

        private void Model_CheckedDriverChanged(object sender, EventArgs e)
        {
            if (_model.CheckedDriver == null)
                CloseDriverInfoPopupWindow();
        }

        private void Model_DriversChanged(object sender, EventArgs e)
        {
            _context.RunOnUiThread(UpdateDrivers);
        }

        private void Model_DriversCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _context.RunOnUiThread(() => ObservableCollectionHelper.ApplyChanges(e, _items));
        }

        private void Model_CurrentLocationChanged(object sender, EventArgs e)
        {
            _context.RunOnUiThread(() =>
            {
                _viewCache.NotifyFillStarted();
                NotifyDataSetChanged();
                _viewCache.NotifyFillFinished();
            });
        }

        private void Map_MapCenterChanged(object sender, EventArgs e)
        {
            _context.RunOnUiThread(() =>
            {
                _viewCache.NotifyFillStarted();
                NotifyDataSetChanged();
                _viewCache.NotifyFillFinished();
            });
        }

        private void Map_MapZoomChanged(object sender, EventArgs e)
        {
            _context.RunOnUiThread(() =>
            {
                _viewCache.NotifyFillStarted();
                NotifyDataSetChanged();
                _viewCache.NotifyFillFinished();
            });
        }
    }
}