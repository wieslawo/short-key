using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WpfAppShortCut.Services;
using WpfAppShortCut.Services.HotKey;

namespace WpfAppShortCut
{
    public partial class MainWindow : Window
    {
        private readonly HotKeyService _hotKeyService;

        public MainWindow()
        {
            _hotKeyService = new HotKeyService();
            InitializeComponent();
        }
       
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hotKeyService.RegisterHotKey(this, OnHotKeyAction);
        }

        void OnHotKeyAction()
        {
            //SearchTextBox.Text = "Ctr+Caps_Lock was pressed";
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        protected override void OnClosed(EventArgs e)
        {
            _hotKeyService.UnregisterHotKey();
            base.OnClosed(e);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MainWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
