using Microsoft.Maui.Controls;

namespace UmbracoAnywhere.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}