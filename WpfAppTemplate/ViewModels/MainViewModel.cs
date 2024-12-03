using System.IO;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace WpfAppTemplate.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ILogger<MainViewModel> logger;
        private ContentViewModel contentTab;

        private RelayCommand loadDataCommand;

        private RelayCommand saveCommand;

        private int selectedTabIndex;

        private bool isDarkTheme;

        public MainViewModel(ContentViewModel content, CultureViewModel cultureViewModel, ILogger<MainViewModel> logger)
        {
            ContentTab = content;

            CultureViewModel = cultureViewModel;
            this.logger = logger;

            var systemTheme = Theme.GetSystemTheme().Value;
            isDarkTheme = systemTheme == BaseTheme.Dark;
        }

        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                SetProperty(ref isDarkTheme, value);

                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();

                var systemColors = GetThemeColorsFromWindowsTheme();
                theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
                theme.SetPrimaryColor(systemColors.PrimaryColor);
                theme.SetSecondaryColor(systemColors.SecondaryColor);
                paletteHelper.SetTheme(theme);
            }
        }

        public ContentViewModel ContentTab
        {
            get => contentTab;
            set => SetProperty(ref contentTab, value);
        }

        public CultureViewModel CultureViewModel { get; }
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);
        public ICommand LoadDataCommand => loadDataCommand ??= new RelayCommand(LoadData);

        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set => SetProperty(ref selectedTabIndex, value);
        }

        private void Save()
        {

        }

        private void LoadData()
        {
            string sourcesDirectory = SelectDirectory();

            if (!Directory.Exists(sourcesDirectory))
            {
                return;
            }

        }

        private string SelectDirectory()
        {
            OpenFolderDialog dialog = new OpenFolderDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Multiselect = false
            };

            dialog.ShowDialog();
            return dialog.FolderName;
        }

        public static (System.Windows.Media.Color PrimaryColor, System.Windows.Media.Color SecondaryColor) GetThemeColorsFromWindowsTheme()
        {
            var colors = SystemParameters.UxThemeColor;
            // Отримуємо кольори з теми Windows
            var accentColor = SystemParameters.WindowGlassColor;
            var PrimaryColor = accentColor;

            // Встановлюємо вторинний колір як більш темний відтінок основного кольору
            var SecondaryColor = System.Windows.Media.Color.Multiply(accentColor, 0.7f);

            return (PrimaryColor, SecondaryColor);
        }
    }
}