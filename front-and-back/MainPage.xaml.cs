using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace front_and_back
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainPageBinding();
            InitializeComponent();
        }
    }
    public enum OneHotVisible
    {
        Front,
        Back,
    }
    class MainPageBinding : INotifyPropertyChanged
    {
        public MainPageBinding()
        {
            TappedCommand = new Command(OnTapped);
        }
        public OneHotVisible OneHotVisible
        {
            get => _oneHotVisible;
            set
            {
                if (!Equals(_oneHotVisible, value))
                {
                    _oneHotVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        OneHotVisible _oneHotVisible = 0;
        public ICommand TappedCommand { get; private set; }
        private void OnTapped(object o)
        {
            OneHotVisible = (OneHotVisible)((((int)OneHotVisible) + 1) % 2);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class OneHotToVisibilityConverterFront : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is OneHotVisible oneHotVisible && oneHotVisible == OneHotVisible.Front);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class OneHotToVisibilityConverterBack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is OneHotVisible oneHotVisible && oneHotVisible == OneHotVisible.Back);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}