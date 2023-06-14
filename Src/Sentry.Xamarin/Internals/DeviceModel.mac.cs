using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCConstants = ObjCRuntime.Constants;

namespace Sentry.Xamarin.Internals
{
    [Preserve(AllMembers = true)]
    internal partial class DeviceModel
    {
        private const string HardwareMachine = "hw.machine";
        private const string HardwareModel = "hw.model";

        [DllImport(ObjCConstants.SystemLibrary)]
        static extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        public string Machine => GetWithSysCtlByName(HardwareMachine, "Unknown");

        public string Model => GetWithSysCtlByName(HardwareModel, "Unknown");

        private string GetWithSysCtlByName(string key, string defaultValue)
        {
            try
            {
                var pLen = Marshal.AllocHGlobal(sizeof(int));
                sysctlbyname(key, IntPtr.Zero, pLen, IntPtr.Zero, 0);

                var length = Marshal.ReadInt32(pLen);

                if (length == 0)
                {
                    Marshal.FreeHGlobal(pLen);
                    return defaultValue;
                }

                var pStr = Marshal.AllocHGlobal(length);
                sysctlbyname(key, pStr, pLen, IntPtr.Zero, 0);

                var hardwareStr = Marshal.PtrToStringAnsi(pStr);

                Marshal.FreeHGlobal(pLen);
                Marshal.FreeHGlobal(pStr);

                return hardwareStr ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}