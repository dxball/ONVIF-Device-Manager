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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyEvents.xaml
	/// </summary>
	public partial class PropertyEvents : BasePropertyControl {
		public PropertyEvents() {
			InitializeComponent();
		}
		public void FillListView(List<odm.controllers.EventDescriptor> lstEvents) {
			//_lviewEvents.Items.Clear();
			//lstEvents.ForEach(x => { AddListItem(x); });
		}
		public void AddListItem(odm.controllers.EventDescriptor evDescr) {
		}
		public void RemoveListViewItem(odm.controllers.EventDescriptor evDescr) {
			//_lviewEvents.Items.ForEach(itm => { if (((ListViewItemBindable)itm).Tag == evDescr) ((ListViewItemBindable)itm).Remove(); });
		}
	}
}
