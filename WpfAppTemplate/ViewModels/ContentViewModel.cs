using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace WpfAppTemplate.ViewModels
{
    public class ContentViewModel : ObservableObject
    {
        private ObservableObject contentTab;

        private int selectedInfo;

        public ContentViewModel(ILogger<ContentViewModel> logger, CultureViewModel cultureViewModel)
        {
            CultureViewModel = cultureViewModel;
        }

        public ObservableObject ContentTab
        {
            get => contentTab;
            set => SetProperty(ref contentTab, value);
        }

        public int SelectedInfo
        {
            get => selectedInfo;
            set => SetProperty(ref selectedInfo, value);
        }
        public CultureViewModel CultureViewModel { get; }

        internal event Action OnSelectedRowChanged;
    }
}