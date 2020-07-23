using System.Collections.Generic;

public class PanglePackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Pangle"; }
    }

    public override string Version
    {
        get { return /*UNITY_PACKAGE_VERSION*/"1.0.0"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, /*ANDROID_SDK_VERSION*/"3.1.0.1" },
                { Platform.IOS, /*IOS_SDK_VERSION*/"3.1.0.4" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.Pangle" },
                { Platform.IOS, "Pangle" }
            };
        }
    }
}
