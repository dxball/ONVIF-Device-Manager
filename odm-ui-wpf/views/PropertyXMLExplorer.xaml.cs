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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using odm.models;
using odm.utils.extensions;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyXMLExplorer.xaml
	/// </summary>
	public partial class PropertyXMLExplorer : BasePropertyControl {
		public PropertyXMLExplorer(DumpModel devModel) {
			InitializeComponent();
			InitControls(devModel);
			Localization();
		}
		PropertyOnvifExplorerStrings strings {
			get {
				return PropertyOnvifExplorerStrings.Instance;
			}
		}
		LinkButtonsStrings titles = new LinkButtonsStrings();
		void InitControls(DumpModel devModel){
			dumpControl.Init(devModel);
		}
		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.onvifExplorer);
			dumpControl.saveDump.CreateBinding(Button.ContentProperty, strings, x => x.save);

		}
		public override void ReleaseUnmanaged() {
		}
		public override void ReleaseAll() { }
	}
}
