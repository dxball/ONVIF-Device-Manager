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
using Saxon.Api;
using System.Xml;

namespace odm.utils.saxon {
	
	[AttributeUsage(AttributeTargets.Class)]
	public class XQueryExtAttribute : Attribute {
		public string prefix = null;
	}


	public static class XQueryExtensions {

		
		public static void SetExternalVariable(this XQueryEvaluator evaluator, QName name, object value) {
			XdmValue xdmVal = null;

			if (value == null) {
				throw new ArgumentNullException("value");
			}

			if(typeof(bool).IsAssignableFrom(value.GetType())){
				xdmVal = new XdmAtomicValue((bool)value);
			} else if(typeof(string).IsAssignableFrom(value.GetType())){
				xdmVal = new XdmAtomicValue((string)value);
			} else if (typeof(decimal).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((decimal)value);
			} else if (typeof(double).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((double)value);
			} else if (typeof(float).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((float)value);
			} else if (typeof(long).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((long)value);
			} else if (typeof(QName).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((QName)value);
			} else if (typeof(Uri).IsAssignableFrom(value.GetType())) {
				xdmVal = new XdmAtomicValue((Uri)value);
			} else if (typeof(XmlNode).IsAssignableFrom(value.GetType())) {
				var proc = new Processor();
				var builder = proc.NewDocumentBuilder();
				xdmVal = builder.Build((XmlNode)value);
			} else if (Attribute.GetCustomAttribute(value.GetType(), typeof(SerializableAttribute)) != null) {
				var xml = value.Serialize();
				var proc = new Processor();
				var builder = proc.NewDocumentBuilder();
				xdmVal = builder.Build(xml);
			}
			
			if (xdmVal == null) {
				throw new ArgumentException("value");
			}
			
			evaluator.SetExternalVariable(name, xdmVal);		    
		}

		public static string GetNameSpace(Type type) {
			string[] asm = type.Assembly.FullName.Split(new char[] { ',' });
			Dictionary<string, string> asm_info = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
			asm_info["Name"] = asm[0];
			for (int i = 1; i < asm.Length; ++i) {
				var s = asm[i].Split(new char[] { '=' }, 2);
				var name = s[0].Trim();
				string value = null;
				if (s.Length > 1) {
					value = s[1].Trim();
				}
				asm_info[name] = value;
			}
			return "clitype:" + type.FullName + "?asm=" + asm_info["Name"];
		}

		public static void DeclareExtension(this XQueryCompiler compiler,string prefix, Type type) {
			compiler.DeclareNamespace(prefix, GetNameSpace(type));
		}

		public static void DeclareExtension(this XQueryCompiler compiler, Type type) {
			var attr = Attribute.GetCustomAttribute(type, typeof(XQueryExtAttribute), false) as XQueryExtAttribute;
			if (attr != null && !String.IsNullOrEmpty(attr.prefix)) {
				compiler.DeclareExtension(attr.prefix, type);
			} else {
				compiler.DeclareExtension(type.Name, type);
			}
		}
		

	}
}
