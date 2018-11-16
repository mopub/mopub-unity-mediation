using System.Collections.Generic;

public class IronSourceAdapterConfig : AdapterConfig
{
    // WARNING: These values are auto-updated by the packaging script.
    private const string _version = "1.0";
    private const string _androidSdkVersion = "6.7.12.0";
    private const string _iosSdkVersion = "6.7.12.0";


    public override string Name
    {
        get { return "IronSource"; }
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
                { Platform.ANDROID, "com.mopub.mobileads.IronSource" },
                { Platform.IOS, "IronSource" }
            };
        }
    }
}
