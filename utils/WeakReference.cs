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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization;


namespace onvifdm.utils {

	/// <summary>
	/// Represents a weak reference, which references an object while still allowing
	/// that object to be reclaimed by garbage collection.
	/// </summary>
	/// <typeparam name="T">The type of the object that is referenced.</typeparam>
	[Serializable]
	public class WeakReference<T> : WeakReference where T : class {
		/// <summary>
		/// Initializes a new instance of the WeakReference{T} class, referencing
		/// the specified object.
		/// </summary>
		/// <param name="target">The object to reference.</param>
		public WeakReference(T target) : base(target) {
		}

		/// <summary>
		/// Initializes a new instance of the WeakReference{T} class, referencing
		/// the specified object and using the specified resurrection tracking.
		/// </summary>
		/// <param name="target">An object to track.</param>
		/// <param name="trackResurrection">Indicates when to stop tracking the object. 
		/// If true, the object is tracked
		/// after finalization; if false, the object is only tracked 
		/// until finalization.</param>
		public WeakReference(T target, bool trackResurrection) : base(target, trackResurrection) {
		}
		protected WeakReference(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
		/// <summary>
		/// Gets or sets the object (the target) referenced by the 
		/// current WeakReference{T} object.
		/// </summary>
		public new T Target {
			get {
				return (T)base.Target;
			}
			set {
				base.Target = value;
			}
		}
	}  
}
