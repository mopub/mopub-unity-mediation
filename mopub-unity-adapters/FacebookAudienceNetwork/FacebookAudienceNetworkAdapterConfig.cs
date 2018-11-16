using System.Collections.Generic;

public class FacebookAudienceNetworkAdapterConfig : AdapterConfig
{
    // WARNING: These values are auto-updated by the packaging script.
    private const string _version = "1.0";
    private const string _androidSdkVersion = "5.1.0.0";
    private const string _iosSdkVersion = "5.1.0.0";


    public override string Name
    {
        get { return "FacebookAudienceNetwork"; }
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
                { Platform.ANDROID, "com.mopub.mobileads.FacebookAudienceNetwork" },
                { Platform.IOS, "FacebookAudienceNetwork" }
            };
        }
    }
}
