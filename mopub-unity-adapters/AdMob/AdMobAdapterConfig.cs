using System.Collections.Generic;

public class AdMobAdapterConfig : AdapterConfig
{
    // WARNING: These values are auto-updated by the packaging script.
    private const string _version = "1.1";
    private const string _androidSdkVersion = "17.0.0.2";
    private const string _iosSdkVersion = "7.35.1.0";


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
                { Platform.ANDROID, _androidSdkVersion },
                { Platform.IOS, _iosSdkVersion }
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
