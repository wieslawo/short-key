using System.ComponentModel;
using System.Windows;

namespace WpfAppShortCut
{
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
 
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ((MainWindow)MainWindow).ShowHideMainWindow();
            _notifyIcon.Icon = WpfAppShortCut.Properties.Resources.MyIcon;
            _notifyIcon.Text = @"keymap - press Ctrl+Caps_Lock to open";
            _notifyIcon.Visible = true;
 
            CreateContextMenu();

            //Temporary work around to load method OnSourceInitialized and activate hot keys
            ((MainWindow) MainWindow)?.ShowHideMainWindow();
            ((MainWindow) MainWindow)?.ShowHideMainWindow();
        }

        private void CreateContextMenu()
        {
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
    }
}
