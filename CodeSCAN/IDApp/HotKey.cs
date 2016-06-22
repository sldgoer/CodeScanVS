using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;  
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IDScanAndLoad
{
    class HotKey
    {
        //要定义热键的窗口的句柄
        //定义热键ID（不能与其它ID重复）int id, 
        //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效KeyModifiers fsModifiers,                 Keys vk                     //定义热键的内容
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr wnd, int id, MODKEY mode, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr wnd, int id);

        [Flags()]
        public enum MODKEY
        {
            None = 0,
            ALT = 0x0001,
            CTRL = 0x0002,
            SHIFT = 0x0004,
            WIN = 0x0008,
        }

    }
}
