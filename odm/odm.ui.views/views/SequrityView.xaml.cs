using System.Windows.Controls;
using odm.ui.viewModels;

namespace odm.ui.views {
    /// <summary>
    /// Interaction logic for SequrityView.xaml
    /// </summary>
    public partial class SequrityView : UserControl {
        public SequrityView(SequrityViewModel viewModel) {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }

}
