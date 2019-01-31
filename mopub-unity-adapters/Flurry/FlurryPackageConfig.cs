using System.Collections.Generic;

public class FlurryPackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Flurry"; }
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
                { Platform.ANDROID, "com.mopub.mobileads.Flurry" },
                { Platform.IOS, "Flurry" }
            };
        }
    }
}
