using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Example.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Make sure we have a successful session
            if (App.ArloSession != null)
            {
                // Get library instance
                var library = new Arlo.ArloMediaLibrary(App.ArloSession);

                // Get list of videos from the last 10 days
                var videos = await library.Load(10);

                // Get the first video and display it
                var firstvideo = videos.FirstOrDefault();
                if (firstvideo != null)
                {
                    video.Source = new Uri(firstvideo.PresignedContentUrl);
                } else
                {
                    var dialog = new MessageDialog("There are no videos available to view from the last 10 days.", "Error");
                    await dialog.ShowAsync();
                    Application.Current.Exit();
                }

            } else
            {
                var dialog = new MessageDialog(App.SessionError, "Error");
                await dialog.ShowAsync();
                Application.Current.Exit();
            }

        }
    }
}
