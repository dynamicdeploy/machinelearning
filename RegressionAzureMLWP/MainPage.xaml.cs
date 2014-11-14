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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace RegressionAzureMLWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            BindControl();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void BindControl()
        {
            try
            {
                CbCylinders.ItemsSource = AutoPredictionAzureMLClient.NumberOfCylinders;

                CbMake.ItemsSource = AutoPredictionAzureMLClient.Make;

                CbBodyStyle.ItemsSource = AutoPredictionAzureMLClient.BodyStyle;
                                            

               
            }
            catch (Exception ex)
            {
                Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(ex.Message);
                md.ShowAsync();
            }
            
        }
        private async void OnPredict(object sender, RoutedEventArgs e)
        {
            try
            {
                TxtPrice.Text = await AutoPredictionAzureMLClient.InvokeRequestResponseService(CbMake.SelectedItem.ToString(), CbBodyStyle.SelectedItem.ToString(),
                    SliderWheelBase.Value, CbCylinders.SelectedItem.ToString(), (int)SliderEngineSize.Value, (int)SliderHP.Value, (int)SliderRPM.Value, (int)SliderMPG.Value);

            }catch(Exception ex)
            {
                Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(ex.Message);
                md.ShowAsync();
            }
        }

     

      
      
    }
}
