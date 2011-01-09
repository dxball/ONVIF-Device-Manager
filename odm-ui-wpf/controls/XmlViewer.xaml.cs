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
using System.Xml;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for XmlViewer.xaml
	/// </summary>
	public partial class XmlViewer : UserControl {
		private XmlDocument m_xmlDocument;
		public XmlViewer() {
			InitializeComponent();
		}

		public XmlDocument xmlDocument {
			get {
				return m_xmlDocument;
			}
			set {
				m_xmlDocument = value;
				BindXMLDocument();
			}
		}

		private void BindXMLDocument() {
			if (m_xmlDocument == null) {
				xmlTree.ItemsSource = null;
				return;
			}

			XmlDataProvider provider = new XmlDataProvider();
			provider.Document = m_xmlDocument;
			Binding binding = new Binding();
			binding.Source = provider;
			binding.XPath = "child::node()";
			xmlTree.SetBinding(TreeView.ItemsSourceProperty, binding);
		}


	}
}
