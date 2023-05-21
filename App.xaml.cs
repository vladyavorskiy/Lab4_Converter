namespace con4;
using System.Globalization;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

        MainPage = new AppShell();
	}
}
