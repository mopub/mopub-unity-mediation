using System.Collections.Generic;

public class TapjoyPackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Tapjoy"; }
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
                { Platform.ANDROID, "com.mopub.mobileads.Tapjoy" },
                { Platform.IOS, "Tapjoy" }
            };
        }
    }
}
