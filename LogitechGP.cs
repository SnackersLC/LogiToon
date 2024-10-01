using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ToontownLGP
{
    public class LogitechGP
    {
        public LogitechGP() {}

        public bool Init() { return LogiLcdInit(Resources.logitech_gp_friendly_name, LOGI_LCD_TYPE_MONO); }

        public bool IsConnected() { return LogiLcdIsConnected(LOGI_LCD_TYPE_MONO); }

        public void Shutdown() { LogiLcdShutdown(); }

        public void Update() { LogiLcdUpdate(); }

        public bool IsActionButtonsPressed() { return LogiLcdIsButtonPressed(LOGI_LCD_MONO_BUTTON_2) || LogiLcdIsButtonPressed(LOGI_LCD_MONO_BUTTON_3); }

        public bool SetBackground(Byte[] out_bytes) {
            for (int i = 0; i < 43; ++i)
            {
                for (int j = 0; j < 160; ++j)
                {
                    Console.Write(out_bytes[i * 160 + j] == 255 ? "*" : " ");
                }
                Console.Write("\n");
            }            
            

            return LogiLcdMonoSetBackground(out_bytes);
        }


        private const int LOGI_LCD_MONO_BUTTON_0 = (0x00000001);
        private const int LOGI_LCD_MONO_BUTTON_1 = (0x00000002);
        private const int LOGI_LCD_MONO_BUTTON_2 = (0x00000004);
        private const int LOGI_LCD_MONO_BUTTON_3 = (0x00000008);
        private const int LOGI_LCD_MONO_WIDTH = 160;
        private const int LOGI_LCD_MONO_HEIGHT = 43;
        private const int LOGI_LCD_TYPE_MONO = (0x00000001);

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool LogiLcdInit(String friendlyName, int lcdType);

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool LogiLcdIsConnected(int lcdType);

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool LogiLcdIsButtonPressed(int button);

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void LogiLcdUpdate();

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void LogiLcdShutdown();

        //	Monochrome	LCD	functions
        [DllImport("LogitechLcdEnginesWrapper")]
        private static extern bool LogiLcdMonoSetBackground(byte[] monoBitmap);

        [DllImport("LogitechLcdEnginesWrapper", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool LogiLcdMonoSetText(int lineNumber, String text);
    }
}
