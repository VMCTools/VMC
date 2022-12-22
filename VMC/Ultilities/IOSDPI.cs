#if UNITY_EDITOR || UNITY_IOS
using UnityEngine.iOS;
namespace VMC.Ultilities.IOS
{
    public class IOSDPI
    {
        public static int dpi
        {
            get
            {
                switch (Device.generation)
                {
                    case DeviceGeneration.iPad1Gen:
                    case DeviceGeneration.iPad2Gen:
                        return 132;
                    case DeviceGeneration.iPhone:
                    case DeviceGeneration.iPhone3G:
                    case DeviceGeneration.iPhone3GS:
                        return 163;
                    case DeviceGeneration.iPad3Gen:
                    case DeviceGeneration.iPad4Gen:
                    case DeviceGeneration.iPad5Gen:
                    case DeviceGeneration.iPad6Gen:
                    case DeviceGeneration.iPad7Gen:
                    case DeviceGeneration.iPadAir1:
                    case DeviceGeneration.iPadAir2:
                    case DeviceGeneration.iPadPro10Inch1Gen:
                    case DeviceGeneration.iPadPro10Inch2Gen:
                    case DeviceGeneration.iPadPro11Inch:
                    case DeviceGeneration.iPadPro1Gen:
                    case DeviceGeneration.iPadPro2Gen:
                    case DeviceGeneration.iPadPro3Gen:
                    case DeviceGeneration.iPadUnknown:
                        return 264;
                    case DeviceGeneration.iPhone4:
                    case DeviceGeneration.iPhone4S:
                    case DeviceGeneration.iPhone5:
                    case DeviceGeneration.iPhone5C:
                    case DeviceGeneration.iPhone5S:
                    case DeviceGeneration.iPhone6:
                    case DeviceGeneration.iPhone6Plus:
                    case DeviceGeneration.iPhone6S:
                    case DeviceGeneration.iPhone7:
                    case DeviceGeneration.iPhone8:
                    case DeviceGeneration.iPhoneSE1Gen:
                    case DeviceGeneration.iPhoneXR:
                    case DeviceGeneration.iPadMini1Gen:
                    case DeviceGeneration.iPadMini2Gen:
                    case DeviceGeneration.iPadMini3Gen:
                    case DeviceGeneration.iPadMini4Gen:
                    case DeviceGeneration.iPadMini5Gen:
                        return 326;
                    case DeviceGeneration.iPhone6SPlus:
                    case DeviceGeneration.iPhone7Plus:
                    case DeviceGeneration.iPhone8Plus:
                        return 401;
                    case DeviceGeneration.iPhoneX:
                    case DeviceGeneration.iPhoneXS:
                    case DeviceGeneration.iPhoneXSMax:
                    case DeviceGeneration.iPhone11:
                    case DeviceGeneration.iPhone11Pro:
                    case DeviceGeneration.iPhone11ProMax:
                        return 458;
                    // These enums not defined in Unity 2018.4.22f1
                    case DeviceGeneration.iPhone12:
                        return 460;
                    case DeviceGeneration.iPhone12Mini:
                        return 476;
                    default:
                        return 460;
                }
            }
        }
    }
}
#endif