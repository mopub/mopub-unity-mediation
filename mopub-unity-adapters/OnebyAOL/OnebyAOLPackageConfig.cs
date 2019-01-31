using System.Collections.Generic;

public class OnebyAOLPackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "OnebyAOL"; }
    }

    public override string Version
    {
        get { return "UNITY_PACKAGE_VERSION"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "ANDROID_SDK_VERSION" },
                { Platform.IOS, "IOS_SDK_VERSION" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.Millenial" },
                { Platform.IOS, "MPMillennial" }
            };
        }
    }
}
