using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppShortCut
{
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {

            EventManager.RegisterClassHandler(typeof(TextBox),
                UIElement.KeyUpEvent,
                new System.Windows.Input.KeyEventHandler(TextBox_KeyUp));

            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
 
            CreateTryMode();

            //Temporary work around to load method OnSourceInitialized and activate hot keys
            ((MainWindow) MainWindow)?.ShowHideMainWindow();
            ((MainWindow) MainWindow)?.ShowHideMainWindow();
        }

        private void CreateTryMode()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ((MainWindow)MainWindow).ShowHideMainWindow();
            _notifyIcon.Icon = WpfAppShortCut.Properties.Resources.MyIcon;
            _notifyIcon.Text = @"keymap - press Ctrl+Caps_Lock to open";
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("keymap search").Click += (s, e) => ((MainWindow)MainWindow)?.ShowHideMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow?.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }
        
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow?.Hide(); 
            }
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            // your event handler here
            e.Handled = true;
            MessageBox.Show(((TextBox)sender).Text);
        }
    }
}
