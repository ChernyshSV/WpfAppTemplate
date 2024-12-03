using System.Windows;
using System.Windows.Controls;
using WpfAppTemplate.ViewModels;

namespace MikroTikAdmin.Views
{
    /// <summary>
    ///     Interaction logic for ContentControl.xaml
    /// </summary>
    public partial class ContentControl : UserControl
    {
        private ContentViewModel? contentVm;


        public ContentControl()
        {
            InitializeComponent();

            DataContextChanged += ContentControl_DataContextChanged;
        }

        private void ContentControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            contentVm = e.NewValue as ContentViewModel;
        }

        private void contentDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}