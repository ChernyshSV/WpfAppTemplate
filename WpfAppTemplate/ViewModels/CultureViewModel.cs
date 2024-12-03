using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfAppTemplate.ViewModels
{
    public class CultureViewModel : ObservableObject
    {
        public event EventHandler<CultureInfo> OnCurrentLanguageChanged;
        public CultureViewModel()
        {
            Languages = new List<CultureInfo>
            {
                new("en-US"), new("uk-UA")
            };

            OnCurrentLanguageChanged += (o, e) => { };

            CurrentLanguage = Thread.CurrentThread.CurrentUICulture;
        }

        public List<CultureInfo> Languages { get; }

        public CultureInfo CurrentLanguage
        {
            get => Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                //if (value == Thread.CurrentThread.CurrentUICulture)
                //	return;

                string propertyName = nameof(CurrentLanguage);
                OnPropertyChanging(propertyName);

                Thread.CurrentThread.CurrentUICulture = value;
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(value.Name);


                Uri dictionaryUri = null;
                if (value.Name == "en-US")
                {
                    dictionaryUri = new Uri("./Resources/lang.xaml", UriKind.Relative);
                }
                else
                {
                    dictionaryUri = new Uri(string.Format("./Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                }

                ResourceDictionary dict = new ResourceDictionary
                {
                    Source = dictionaryUri
                };

                ResourceDictionary? oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                               where d.Source != null && d.Source.OriginalString.StartsWith("./Resources/lang.")
                                               select d).First();

                foreach (object? d in oldDict.Keys)
                {
                    if (dict.Contains(d))
                    {
                        continue;
                    }

                    dict.Add(d, oldDict[d]);
                }

                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                OnPropertyChanged(propertyName);

                OnCurrentLanguageChanged(this, value);
            }
        }

        public string GetString(string key)
        {
            ResourceDictionary? dict = Application.Current.Resources.MergedDictionaries[0];
            if (dict.Contains(key))
            {
                return (string)dict[key];
            }

            throw new NotImplementedException($"resource {key}, not found");
        }
    }
}