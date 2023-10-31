As I understand your description, one might start with a grid with a header row, and a footer row, and then a middle row where the `IView` images do their flip. One approach that has worked for me is to implement a "one-hot" by placing another grid in that middle row, and setting a tap gesture to advance a `OneHotVisible` enumeration. In this sample, I'm just using `Front` and `Back` but you can have any number of values.

[![front and back alternate][1]][1]


The IView objects, like front and back, are just piled on top of each other in that middle Grid, but only one is ever visible because changes of the `OneHotVisible` property are translated using `IValueConverter` to tell each view whether to show itself or not.
___

```
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:front_and_back"
             x:Class="front_and_back.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:OneHotToVisibilityConverterFront x:Key="FrontConverter" />
            <local:OneHotToVisibilityConverterBack x:Key="BackConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>

    <Grid
        x:Name="TableLayoutPanel"
        Padding="30,0">
        <Grid.RowDefinitions>
            <RowDefinition Height="80"/>
            <RowDefinition/>
            <RowDefinition Height="80"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
        </Grid.ColumnDefinitions>

        <Label 
            Grid.Row="0"
            Text="Header"
            FontSize="32"
            HorizontalOptions="Center" />
        <Grid   
            Grid.Row="1">
            <Grid.GestureRecognizers>
                <TapGestureRecognizer Command="{Binding TappedCommand}" />
            </Grid.GestureRecognizers>
            <Label
                IsVisible="{Binding OneHotVisible, Converter={StaticResource FrontConverter}}"
                Text="Front!"
                VerticalTextAlignment="Center"
                HorizontalTextAlignment="Center"
                BackgroundColor="LightBlue"
                FontSize="32"
                HorizontalOptions="Fill" />            
            <Label
                IsVisible="{Binding OneHotVisible, Converter={StaticResource BackConverter}}"
                Text="Back!"
                VerticalTextAlignment="Center"
                HorizontalTextAlignment="Center"
                BackgroundColor="LightGreen"
                FontSize="32"
                HorizontalOptions="Fill" />
        </Grid>
        <Label 
            Grid.Row="2"
            Text="Footer"
            FontSize="32"
            HorizontalOptions="Center" />
    </Grid>

</ContentPage>
```

**Main Page**

```
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        BindingContext = new MainPageBinding();
        InitializeComponent();
    }
}
```
___

**Main View Model**

```

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
```

___

**IValueConverters**

```
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
```

I hope this might offer a starting point to experiment with.

  [1]: https://i.stack.imgur.com/6YbPk.png