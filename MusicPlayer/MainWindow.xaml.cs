using GalaSoft.MvvmLight.Messaging;
using Hardcodet.Wpf.TaskbarNotification;
using MusicPlayer.Models;
using System.Windows;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainConfig mainConfig;

        public MainWindow()
        {
            InitializeComponent();

            SetMessaging();

            mainConfig = FileSupport.AppConfiguration.LoadMainConfig();
            this.Width = mainConfig.Width;
            this.Height = mainConfig.Height;
            this.Left = mainConfig.Left;
            this.Top = mainConfig.Top;
        }

        private void SetMessaging()
        {
            Messenger.Default.Register<NotifyMessage>(this, (message) =>
            {
                tb.ShowBalloonTip(message.Title, message.Message, BalloonIcon.Info);
            });

            Messenger.Default.Register<AlertMessageMessage>(this, (message) =>
            {
                MessageBox.Show(message.Message, message.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            });

            Messenger.Default.Register<CloseAppMessage>(this, (message) =>
            {
                Application app = Application.Current;
                app.Shutdown();
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save settings
            mainConfig.Height = this.Height;
            mainConfig.Width = this.Width;
            mainConfig.Left = this.Left;
            mainConfig.Top = this.Top;
            FileSupport.AppConfiguration.SaveMainConfig(mainConfig);

            Messenger.Default.Send<AppClosingMessage>(new AppClosingMessage());
        }
    }
}
