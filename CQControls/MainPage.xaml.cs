using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CQControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainVM VM { get; set; } = new MainVM();
        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested; ;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                e.Handled = true;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = e.AddedItems[0] as PageModel;
            this.Frame.Navigate(model.PageType);
        }
    }
    public class PageModel
    {
        public Type PageType { get; set; }
        public string PageName { get; set; }
        public string PageDsc { get; set; }
    }
    public class MainVM
    {
        public ObservableCollection<PageModel> Pages { get; set; }
        public MainVM()
        {
            Pages = new ObservableCollection<PageModel>();
            Pages.Add(new PageModel { PageName = "ToolTip Control", PageType = typeof(CQControls.CQToolTipDemo) ,PageDsc="Click to get a demo"});
        }
    }
}
