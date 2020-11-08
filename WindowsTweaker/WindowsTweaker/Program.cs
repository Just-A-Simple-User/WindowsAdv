using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace WindowsTweaker
{
    using SLID = Guid;
    class Program
    {
        public static void BlueConsole(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
        }
        public static string activationstatus;
        public static string OSBit;
        static void Main(string[] args)
        {
            Checking();
            Is64bitOS();
            Console.Title = "Windows Advanced ";
            string textToEnter = "WindowsAdv 1 [DEBUGGING VERSION / ONLY DEVELOPERS]";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (textToEnter.Length / 2)) + "}", textToEnter));
            Console.WriteLine("\n\n\n");
            Console.WriteLine("[Informations]\n===========================");
            Console.WriteLine("Version 1.0");
            Console.WriteLine("Windows Status: " + activationstatus);
            string releaseId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString();
            Console.WriteLine("Windows: " + (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion", "ProductName", null) + " " + releaseId);
            Console.WriteLine("Arhitecture: " + OSBit);
            Console.WriteLine("Logged in as: " + Environment.UserName);
            Console.WriteLine("Computer name: " +  Environment.MachineName);
            Console.WriteLine("===========================");
            Menu();
            Console.ReadLine();
        }

        public static void Menu()
        {
            BlueConsole("\n\nTweaks\n===========================");
            BlueConsole("[1] Debloat");
        }


        public static void Is64bitOS()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                OSBit = "x64";
            } else
            {
                OSBit = "x32";
            }
        }
        public enum SL_GENUINE_STATE
        {
            SL_GEN_STATE_IS_GENUINE = 0,
            SL_GEN_STATE_INVALID_LICENSE = 1,
            SL_GEN_STATE_TAMPERED = 2,
            SL_GEN_STATE_LAST = 3
        }

        [DllImportAttribute("Slwga.dll", EntryPoint = "SLIsGenuineLocal", CharSet = CharSet.None, ExactSpelling = false, SetLastError = false, PreserveSig = true, CallingConvention = CallingConvention.Winapi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        [PreserveSigAttribute()]
        internal static extern uint SLIsGenuineLocal(ref SLID slid, [In, Out] ref SL_GENUINE_STATE genuineState, IntPtr val3);

        public static bool IsGenuineWindows()
        {
            bool _IsGenuineWindows = false;
            Guid ApplicationID = new Guid("55c92734-d682-4d71-983e-d6ec3f16059f"); //Application ID GUID http://technet.microsoft.com/en-us/library/dd772270.aspx
            SLID windowsSlid = (Guid)ApplicationID;
            try
            {
                SL_GENUINE_STATE genuineState = SL_GENUINE_STATE.SL_GEN_STATE_LAST;
                uint ResultInt = SLIsGenuineLocal(ref windowsSlid, ref genuineState, IntPtr.Zero);
                if (ResultInt == 0)
                {
                    _IsGenuineWindows = (genuineState == SL_GENUINE_STATE.SL_GEN_STATE_IS_GENUINE);
                }
                else
                {
                    Console.WriteLine("Error getting information {0}", ResultInt.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _IsGenuineWindows;
        }

        static void Checking()
        {
            if (Environment.OSVersion.Version.Major >= 6) //Version 6 can be Windows Vista, Windows Server 2008, or Windows 7
            {
                if (IsGenuineWindows())
                {
                    activationstatus = "Activated";
                }
                else
                {
                    activationstatus = "Not activated";
                }
            }
            else
            {
                activationstatus = "OS not supported";
            }
        }
    }
}
