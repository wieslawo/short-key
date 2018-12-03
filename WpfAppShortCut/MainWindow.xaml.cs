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
            Loaded += Window_Loaded;
        }
       
        protected override void OnSourceInitialized(EventArgs e)
        {
            _hotKeyService.RegisterHotKey(this, OnHotKeyAction);
            base.OnSourceInitialized(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SearchTextBox.Focus();
        }

        void OnHotKeyAction()
        {
            ShowHideMainWindow();
        }

        public void ShowHideMainWindow()
        {
            if (IsVisible)
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                    Activate();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                Show();
            }
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
