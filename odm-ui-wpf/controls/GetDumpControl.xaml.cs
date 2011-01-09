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
using odm.onvif;
using System.Threading;
using System.Xml;
using odm.utils;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for GetDumpControl.xaml
	/// </summary>
	public partial class GetDumpControl : UserControl {
		public GetDumpControl() {
			InitializeComponent();
		}

		public void InitModel(Session curSession) {
			DumpModel onvifDump = new DumpModel();
			onvifDump.Load(curSession)
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(arg => {
					xmlView.xmlDocument = arg.xmlDump;
				}, err => {
					dbg.Error(err);
				});
		}
	}
}