using System.Collections.Generic;

public class OnebyAOLAdapterConfig : AdapterConfig
{
    // WARNING: These values are auto-updated by the packaging script.
    private const string _version = "1.0";
    private const string _androidSdkVersion = "6.8.2.0";
    private const string _iosSdkVersion = "6.8.1.0";


    public override string Name
    {
        get { return "OnebyAOL"; }
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
                { Platform.ANDROID, "com.mopub.mobileads.Millenial" },
                { Platform.IOS, "MPMillennial" }
            };
        }
    }
}
