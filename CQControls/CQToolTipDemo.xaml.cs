using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CQControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class CQToolTipDemo : Page
    {
        public CQToolTipDemo()
        {
            this.InitializeComponent();

            InitData();
        }

        public void InitData()
        {

            var horizentalNames = Enum.GetNames(typeof(HorizontalAlignment));
            foreach (var item in horizentalNames)
            {
                this.horizentalList.Items.Add(item);
            }

            var verticalNames = Enum.GetNames(typeof(VerticalAlignment));
            foreach (var item in verticalNames)
            {
                this.verticalList.Items.Add(item);
            }
        }

        private void horizentalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = e.AddedItems[0] as string;
            var value = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), name);
            cqtooltip.HorizontalAlignment = value;
        }

        private void verticalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = e.AddedItems[0] as string;
            var value = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), name);
            cqtooltip.VerticalAlignment = value;
        }
    }
}
