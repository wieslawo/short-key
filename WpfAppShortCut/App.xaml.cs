using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using WpfAppShortCut.Services.HotKey;
using WpfAppShortCut.Services.NativeWin32;

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
            _notifyIcon.Text = @"keymap - press Ctrl+Spacebar to open";
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("keymap search      Ctrl+Spacebar").Click += (s, e) => ((MainWindow)MainWindow)?.ShowHideMainWindow();
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

            //var allProcesses = Process.GetProcesses();

            //foreach (Process theprocess in allProcesses)
            //{
            //    Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
            //}

            //Process[] process1 = Process.GetProcessesByName("chrome");
            //process1[0].WaitForInputIdle();
            //var handler = NativeWin32.FindWindow("chrome", null);
            var iHandle = NativeWin32.FindWindow("Notepad++", null);
            //var iHandle = FindWindow("Notepad++", null);
            //var iHandle = NativeWin32.FindWindow(null,"");

            NativeWin32.SetForegroundWindow(iHandle);
            System.Windows.Forms.SendKeys.SendWait("{F11}");

            //SendMessage(iHandle, NativeWin32.WM_KEYDOWN, NativeWin32.VK_A, NativeWin32.VK_SHIFT);
            //var mes = SendMessage(iHandle, NativeWin32.WM_KEYDOWN, new IntPtr(NativeWin32.VK_F11), IntPtr.Zero);
            //var mes2 = NativeWin32.SendMessage(iHandle, NativeWin32.SC_CLOSE, 0, 0);


            e.Handled = true;
            //MessageBox.Show(((TextBox)sender).Text);
        }
    }
}
