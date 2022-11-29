using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using UmbracoAnywhere.Maui.Models;

namespace UmbracoAnywhere.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly IContentService _contentService;

        public MainPage(IContentService contentService)
        {
            _contentService = contentService;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var collection = new ObservableCollection<ListViewItem>();

            var items = _contentService.GetRootContent();

            foreach (var item in items)
            {
                collection.Add(new ListViewItem
                {
                    Name = item.Name
                });
            }

            ItemListView.ItemsSource = collection;
        }
    }
}