using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfAppShortCut.Services.HotKey
{
    public class HotKeyService
    {
        private const int HotkeyId = 9000;
        private IntPtr _windowHandle;
        private HwndSource _source;
        private Action _hotKeyAction;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
 
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
 
        internal void RegisterHotKey(MainWindow mainWindow, Action hotKeyAction)
        {
            _windowHandle = new WindowInteropHelper(mainWindow).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source?.AddHook(HwndHook);
            _hotKeyAction = hotKeyAction;
 
            RegisterHotKey(_windowHandle, HotkeyId, HotKeyModifiers.MOD_CONTROL, VirtualKeyCodes.VK_SPACE); //CTRL + SPACEBAR
        }

        internal void UnregisterHotKey()
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HotkeyId);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HotkeyId:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VirtualKeyCodes.VK_SPACE)
                            {
                                _hotKeyAction();
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

    }
}
