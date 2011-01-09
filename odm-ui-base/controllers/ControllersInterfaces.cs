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
using System.Windows.Forms;
using odm.controls;
using System.ComponentModel;
using odm.onvif;
using odm.models;

namespace odm.controllers {
	public class DataGridViewCheckBoxCellBindable : DataGridViewCheckBoxCell, IBindableComponent {
		BindingContext _bindingContext;
		public void Dispose() { }
		public ISite Site { get { return null; } set { } }
		public event EventHandler Disposed;
		public BindingContext BindingContext {
			get {
				if (_bindingContext == null)
					_bindingContext = new BindingContext();
				return _bindingContext;
			}
			set {
				_bindingContext = value;
			}
		}
		ControlBindingsCollection _dataBindings;
		public ControlBindingsCollection DataBindings {
			get {
				if (_dataBindings == null) {
					_dataBindings = new ControlBindingsCollection(this);
				}
				return _dataBindings;
			}
		}
	}
	public class DataGridViewTextBoxCellBindable : DataGridViewTextBoxCell, IBindableComponent {
		BindingContext _bindingContext;
		public void Dispose() { }
		public ISite Site { get { return null; } set { } }
		public event EventHandler Disposed;
		public BindingContext BindingContext {
			get {
				if (_bindingContext == null)
					_bindingContext = new BindingContext();
				return _bindingContext;
			}
			set {
				_bindingContext = value;
			}
		}
		ControlBindingsCollection _dataBindings;
		public ControlBindingsCollection DataBindings {
			get {
				if (_dataBindings == null) {
					_dataBindings = new ControlBindingsCollection(this);
				}
				return _dataBindings;
			}
		}
	}
	public class ListViewSubItemBindeble : System.Windows.Forms.ListViewItem.ListViewSubItem, IBindableComponent{
		BindingContext _bindingContext;
		public void Dispose() { }
		public ISite Site { get { return null; } set { } }
		public event EventHandler Disposed;
		public BindingContext BindingContext {
			get {
				if (_bindingContext == null)
					_bindingContext = new BindingContext();
				return _bindingContext;
			}
			set {
				_bindingContext = value;
			}
		}
		ControlBindingsCollection _dataBindings;
		public ControlBindingsCollection DataBindings {
			get {
				if (_dataBindings == null) {
					_dataBindings = new ControlBindingsCollection(this);
				}
				return _dataBindings;
			}
		}
	}
	public class ListViewItemBindable : ListViewItem, IBindableComponent {
		BindingContext _bindingContext;
		public void Dispose() { }
		public ISite Site { get { return null; } set { } }
		public event EventHandler Disposed;
		public BindingContext BindingContext {
			get {
				if (_bindingContext == null)
					_bindingContext = new BindingContext();
				return _bindingContext;
			}
			set {
				_bindingContext = value;
			}
		}
		ControlBindingsCollection _dataBindings;
		public ControlBindingsCollection DataBindings {
			get {
				if (_dataBindings == null) {
					_dataBindings = new ControlBindingsCollection(this);
				}
				return _dataBindings;
			}
		}
	}
	public class ColumnHeaderBindable : ColumnHeader, IBindableComponent {
		BindingContext _bindingContext;
		public BindingContext BindingContext {
			get {
				if (_bindingContext == null)
					_bindingContext = new BindingContext();
				return _bindingContext;
			}
			set {
				_bindingContext = value;
			}
		}
		ControlBindingsCollection _dataBindings;
		public ControlBindingsCollection DataBindings {
			get {
				if (_dataBindings == null) {
					_dataBindings = new ControlBindingsCollection(this);
				}
				return _dataBindings;
			}
		}
	}

	public interface IRelesable {
		void ReleaseAll();
	}
	//public interface IPropertyController {
	//    BasePropertyControl CreateController(Session session, ChannelDescription channel);
	//}
}
