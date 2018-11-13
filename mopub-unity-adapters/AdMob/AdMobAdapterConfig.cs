using System.Collections.Generic;

public class AdMobAdapterConfig : AdapterConfig
{
    private const string _version = "1.0";

    public override string Name
    {
        // TODO: change back to "AdMob" when SDK Manager uses new keyname field
        get { return "Google(AdMob)"; }
    }

    public override string Version
    {
        get { return _version; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "17.0.0" },
                { Platform.IOS, "7.35.1" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.GooglePlayServices" },
                { Platform.IOS, "MPGoogleAdMob" }
            };
        }
    }
}
