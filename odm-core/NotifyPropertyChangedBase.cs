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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Forms;

using System.Threading;
using System.Concurrency;
using System.Reflection;

namespace odm.utils {

	
	public static class BindingExtensions {
		private class AnonymousBinding<TControlProperty, TSourceProperty> : Binding {
			Func<TSourceProperty, TControlProperty> m_formater = null;
			public AnonymousBinding(string propertyName, object dataSource, string dataMember, Func<TSourceProperty, TControlProperty> formater)
				: base(propertyName, dataSource, dataMember, true) {
				this.m_formater = formater;
			}
			protected override void OnFormat(ConvertEventArgs cevent) {
				if(m_formater==null){
					base.OnFormat(cevent);
					return;
				}
				dbg.Assert(cevent.Value.GetType() == typeof(TControlProperty));
				var value = (TSourceProperty)cevent.Value;

				cevent.Value = m_formater(value);
			}

		}
		public static TControl CreateBinding<TControl, TControlProperty, TSource, TSourceProperty>(this TControl control, Expression<Func<TControl, TControlProperty>> propExpr, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr)
			where TControl : IBindableComponent
			where TSource : INotifyPropertyChanged
			//where TControlProperty : TSourceProperty 
		{
			var data_member = dataExpr.Body as MemberExpression;
			dbg.Assert(data_member != null);

			var prop_member = propExpr.Body as MemberExpression;
			dbg.Assert(prop_member != null);
			var binding = new Binding(prop_member.Member.Name, dataSource, data_member.Member.Name, false, DataSourceUpdateMode.OnPropertyChanged);
			control.DataBindings.Add(binding);
			return control;
		}

		public static TControl CreateBinding<TControl, TControlProperty, TSource, TSourceProperty>(this TControl control, Expression<Func<TControl, TControlProperty>> propExpr, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr, DataSourceUpdateMode updateModel)
			where TControl : IBindableComponent
			where TSource : INotifyPropertyChanged
			//where TControlProperty : TSourceProperty 
		{
			var data_member = dataExpr.Body as MemberExpression;
			dbg.Assert(data_member != null);

			var prop_member = propExpr.Body as MemberExpression;
			dbg.Assert(prop_member != null);

			var binding = new Binding(prop_member.Member.Name, dataSource, data_member.Member.Name);
			binding.DataSourceUpdateMode = updateModel;
			control.DataBindings.Add(binding);
			return control;
		}

		public static TControl CreateBinding<TControl, TControlProperty, TSource, TSourceProperty>(this TControl control, Expression<Func<TControl, TControlProperty>> propExpr, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr, Func<TSourceProperty, TControlProperty> formater)
			where TControl : IBindableComponent
			where TSource : INotifyPropertyChanged
			//where TControlProperty : TSourceProperty 
		{
			var data_member = dataExpr.Body as MemberExpression;
			dbg.Assert(data_member != null);

			var prop_member = propExpr.Body as MemberExpression;
			dbg.Assert(prop_member != null);

			var binding = new Binding(prop_member.Member.Name, dataSource, data_member.Member.Name);
			binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
			if (formater != null) {
				binding.Format += (sender, cevent) => {
					dbg.Assert(cevent.Value.GetType() == typeof(TSourceProperty));
					var value = (TSourceProperty)cevent.Value;
					cevent.Value = formater(value);
					cevent.Value = formater(value);
				};
			}
			control.DataBindings.Add(binding);
			return control;
		}

		public static TControl CreateBinding<TControl, TControlProperty, TSource, TSourceProperty>(this TControl control, Expression<Func<TControl, TControlProperty>> propExpr, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr, Func<TSourceProperty, TControlProperty> formater, Func<TControlProperty, TSourceProperty> parser)
			where TControl : IBindableComponent
			where TSource : INotifyPropertyChanged
			//where TControlProperty : TSourceProperty 
		{
			var data_member = dataExpr.Body as MemberExpression;
			dbg.Assert(data_member != null);

			var prop_member = propExpr.Body as MemberExpression;
			dbg.Assert(prop_member != null);

			var binding = new Binding(prop_member.Member.Name, dataSource, data_member.Member.Name);
			if (formater != null) {
				binding.Format += (sender, cevent) => {

					dbg.Assert(cevent.Value.GetType() == typeof(TSourceProperty));
					var value = (TSourceProperty)cevent.Value;
					cevent.Value = formater(value);
				};
			}
			if (parser != null) {
				binding.Parse += (sender, cevent) => {

					dbg.Assert(cevent.Value.GetType() == typeof(TControlProperty));
					var value = (TControlProperty)cevent.Value;
					cevent.Value = parser(value);
				};
			}
			control.DataBindings.Add(binding);
			return control;
		}
	}

	public class NotifyPropertyChangedBase : INotifyPropertyChanged {
		private SynchronizationContext m_syncCtx;

		public NotifyPropertyChangedBase() {
			m_syncCtx = SynchronizationContext.Current;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(String propertyName) {
			if (PropertyChanged != null) {
				if (m_syncCtx != null && m_syncCtx!= SynchronizationContext.Current) {
					m_syncCtx.Post(state => PropertyChanged(this, new PropertyChangedEventArgs(propertyName)), null);
				} else {
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
	}

	public class NotifyPropertyChangedBase<T> : INotifyPropertyChanged {
		public NotifyPropertyChangedBase() {
			dbg.Assert(SynchronizationContext.Current != null);
			m_syncCtx = SynchronizationContext.Current;
			if (SynchronizationContext.Current != null) {
				m_scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
			} else {
				m_scheduler = Scheduler.Immediate;
			}
		}
		private SynchronizationContext m_syncCtx = null;
		private IScheduler m_scheduler;
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
			if (m_syncCtx == SynchronizationContext.Current) {
				if (PropertyChanged != null) {
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			} else {
				m_scheduler.Schedule(() => {
					if (PropertyChanged != null) {
						PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
					}
				});
			}
		}
		protected void NotifyPropertyChanged<TProperty>(Expression<Func<T, TProperty>> expression) {
			var me = expression.Body as MemberExpression;
			dbg.Assert(me != null);
			NotifyPropertyChanged(me.Member.Name);
		}
	}
}
