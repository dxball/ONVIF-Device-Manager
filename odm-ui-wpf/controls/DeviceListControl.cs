#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;

using odm.models;
using odm.utils;
using System.Disposables;

namespace odm.ui.controls {

	public class DeviceDescriptionCollection : ObservableCollection<IDeviceDescriptionModel> {
	}

	public class DeviceListControl : Control {
		public static DependencyProperty DevicesProperty = DependencyProperty.Register("Devices", typeof(DeviceDescriptionCollection), typeof(DeviceListControl));
		private static FrameworkPropertyMetadata SelectedDevicePropertyMetadata = new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DeviceListControl.OnSelectedDeviceChanged));
		public static DependencyProperty SelectedDeviceProperty = DependencyProperty.Register("SelectedDevice", typeof(Object), typeof(DeviceListControl), SelectedDevicePropertyMetadata);
#if DEBUG
		private int m_subscriptons = 0;
#endif
		public DeviceListControl() {
		}

		public DeviceDescriptionCollection Devices {
			get {
				return (DeviceDescriptionCollection)GetValue(DevicesProperty);
			}
			set {
				SetValue(DevicesProperty, value);
			}
		}
		public IDeviceDescriptionModel SelectedDevice {
			get {
				return (IDeviceDescriptionModel)GetValue(SelectedDeviceProperty);
			}
			set {
				SetValue(SelectedDeviceProperty, value);
			}
		}

		public IObservable<IDeviceDescriptionModel> SelectedDeviceObservable {
			get {
				var dpd = DependencyPropertyDescriptor.FromProperty(SelectedDeviceProperty, typeof(DeviceListControl));

				return Observable.CreateWithDisposable<IDeviceDescriptionModel>(observer => {
#if DEBUG
					++m_subscriptons;
					dbg.Assert(m_subscriptons<5);
#endif
					observer.OnNext(SelectedDevice);
					var subscription = m_SelectedDeviceSubject.Subscribe(observer);
					return Disposable.Create(()=>{
#if DEBUG
						--m_subscriptons;
						dbg.Assert(m_subscriptons >= 0);
#endif
						subscription.Dispose();
					});
				});
			}
		}
		Subject<IDeviceDescriptionModel> m_SelectedDeviceSubject = new Subject<IDeviceDescriptionModel>();
		protected virtual void OnSelectedDeviceChanged(IDeviceDescriptionModel oldValue, IDeviceDescriptionModel newValue) {
			m_SelectedDeviceSubject.OnNext(newValue);
		}

		private static void OnSelectedDeviceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
			((DeviceListControl)obj).OnSelectedDeviceChanged((IDeviceDescriptionModel)args.OldValue, (IDeviceDescriptionModel)args.NewValue);
		}
	}
}
