using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

using MoPubReward = MoPubManager.MoPubReward;

public class MoPubDemoGUI : MonoBehaviour
{
    private int _selectedToggleIndex;
    private string[] _bannerAdUnits;
    private string[] _interstitialAdUnits;
    private string[] _rewardedVideoAdUnits;
    private string[] _rewardedRichMediaAdUnits;
    private Dictionary<string, List<MoPubReward>> _adUnitToRewardsMapping =
        new Dictionary<string, List<MoPubReward>> ();
    private Dictionary<string, bool> _adUnitToLoadedMapping =
        new Dictionary<string, bool> ();
    private Dictionary<string, bool> _bannerAdUnitToShownMapping =
        new Dictionary<string, bool> ();

    // Workaround for lacking adUnit from onAdLoadedEvent for Banners
    private Queue<string> _requestedBannerAdUnits = new Queue<string> ();

    private string[] _networkList = new string[] {
        "AdColony",
        "AdMob",
        "AOL",
        "AudienceNetwork",
        "Chartboost",
        "Flurry",
        "TapJoy",
        "UnityAds",
        "Vungle"
    };

    #if UNITY_ANDROID
    private Dictionary<string, string[]> _bannerDict = new Dictionary<string, string[]> () {
        { "AdMob", new string[] { "95031808b2a6413da9f0a3543fd29bf7" } },
        { "AOL", new string[] { "1aa442709c9f11e281c11231392559e4" } },
        { "AudienceNetwork", new string[] { "b40a96dd275e4ce5be2cdf5faa92007d" } }
    };

    private Dictionary<string, string[]> _interstitialDict = new Dictionary<string, string[]> () {
        { "AdColony", new string[] { "3aa79f11389540db8e250a80e4d16a46" } },
        { "AdMob", new string[] { "b83d52ca5f864e84a32e58c85086e6b2" } },
        { "AOL", new string[] { "7c8428e5acf94811b03d5d788e6b4c45" } },
        { "AudienceNetwork", new string[] { "9792d876011f4359887d2d26380e8a84" } },
        { "Chartboost", new string[] { "376366b49d324dedae3d5edb360c27b4" } },
        { "Flurry", new string[] { "a56eac0bacb8477f9c959b5364f8e569" } },
        { "TapJoy", new string[] { "81e9878e71b54d24bb220f42d702ff6f" } },
        { "Vungle", new string[] { "4f5e1e97f87c406cb7878b9eff1d2a77" } }
    };

    private Dictionary<string, string[]> _rewardedVideoDict = new Dictionary<string, string[]> () {
        { "AdColony", new string[] { "e258c916e659447d9d98256a3ab2979e" } },
        { "AdMob", new string[] { "49a4a502ed2945fd92bc5798c9421a57" } },
        { "AudienceNetwork", new string[] { "7220c37b8e93499a8f0aff2eb4f0ad3d" } },
        { "Chartboost", new string[] { "df605ab15b56400285c99e521ecc2cb1" } },
        { "TapJoy", new string[] { "cbbcefe882e940daaab7fb6e15c6d2ec" } },
        { "Vungle", new string[] { "6f21f1edd97944a185df00e850d61a98" } }
    };

    private Dictionary<string, string[]> _rewardedRichMediaDict = new Dictionary<string, string[]> () {
    };

    #elif UNITY_IPHONE
    private Dictionary<string, string[]> _bannerDict = new Dictionary<string, string[]> () {
        { "AdMob", new string[] { "c9c2ea9a8e1249b68496978b072d2fd2" } },
        { "AOL", new string[] { "b506db1f3e054c78bff513f188727748" } },
        { "AudienceNetwork", new string[] { "446dfa864dcb4469965267694a940f3d" } }
    };

    private Dictionary<string, string[]> _interstitialDict = new Dictionary<string, string[]> () {
        { "AdMob", new string[] { "744e217f8adc4dec89c87481c9c4006a" } },
        { "AOL", new string[] { "93c3fc00fbb54825b6a33b20927315f7" } },
        { "AudienceNetwork", new string[] { "cec4c5ea0ff140d3a15264da23449f97" } },
        { "Chartboost", new string[] { "a425ff78959911e295fa123138070049" } },
        { "Flurry", new string[] { "49960150e2874e9294105af00a77b85c" } },
        { "TapJoy", new string[] { "83b8ccba00de4d27a2b6d97a3a0d80ce" } },
        { "Vungle", new string[] { "20e01fce81f611e295fa123138070049" } }
    };

    private Dictionary<string, string[]> _rewardedVideoDict = new Dictionary<string, string[]> () {
        { "AdColony", new string[] { "52aa460767374250a5aa5174c2345be3" } },
        { "AdMob", new string[] { "0ceacb73895748ceadf0048a1f989855" } },
        { "AOL", new string[] { "1908cd1ff0934f69bac04c316accc854" } },
        { "AudienceNetwork", new string[] { "5a138cf1a03643ca851647d2b2e20d0d" } },
        { "Chartboost", new string[] { "8be0bb08fb4f4e90a86416c29c235d4a" } },
        { "TapJoy", new string[] { "db8b4f8134664c4d91d3196a53c5aad9" } },
        { "UnityAds", new string[] { "676a0fa97aca48cbbe489de5b2fa4cd1" } },
        { "Vungle", new string[] { "48274e80f11b496bb3532c4f59f28d12" } }
    };

    private Dictionary<string, string[]> _rewardedRichMediaDict = new Dictionary<string, string[]> () {
    };

    #else
    private Dictionary<string, string[]> _bannerDict = new Dictionary<string, string[]> ();
    private Dictionary<string, string[]> _interstitialDict = new Dictionary<string, string[]> ();
    private Dictionary<string, string[]> _rewardedVideoDict = new Dictionary<string, string[]> ();
    private Dictionary<string, string[]> _rewardedRichMediaDict = new Dictionary<string, string[]> ();
    #endif

    // Label style for no ad unit messages
    private GUIStyle _smallerFont;

    // Buffer space between sections
    private int _sectionMarginSize;

    // Currently selected network
    private string _network;

    // Default text for custom data fields
    private static string _customDataDefaultText = "Optional custom data";

    // String to fill with custom data for Rewarded Videos
    private string _rvCustomData = _customDataDefaultText;

    // String to fill with custom data for Rewarded Rich Media
    private string _rrmCustomData = _customDataDefaultText;


    private static bool IsAdUnitArrayNullOrEmpty (string[] adUnitArray) {
        return (adUnitArray == null || adUnitArray.Length == 0);
    }


    private void addAdUnitsToStateMaps (string[] adUnits) {
        foreach (string adUnit in adUnits) {
            _adUnitToLoadedMapping.Add (adUnit, false);
            // Only banners need this map, but init for all to keep it simple
            _bannerAdUnitToShownMapping.Add (adUnit, false);
        }
    }


    public void loadAvailableRewards (string adUnitId, List<MoPubReward> availableRewards) {
        // Remove any existing available rewards associated with this AdUnit from previous ad requests
        _adUnitToRewardsMapping.Remove (adUnitId);

        if (availableRewards != null) {
            _adUnitToRewardsMapping[adUnitId] = availableRewards;
        }
    }


    public void bannerLoaded () {
        if (_requestedBannerAdUnits.Count > 0) {
            string firstRequestedBannerAdUnit = _requestedBannerAdUnits.Dequeue ();
            _adUnitToLoadedMapping[firstRequestedBannerAdUnit] = true;
            _bannerAdUnitToShownMapping[firstRequestedBannerAdUnit] = true;
        }
    }


    public void adLoaded (string adUnit) {
        _adUnitToLoadedMapping[adUnit] = true;
    }


    public void adDismissed (string adUnit) {
        _adUnitToLoadedMapping[adUnit] = false;
    }


    void Start () {
        var allBannerAdUnits = new string[0];
        var allInterstitialAdUnits = new string[0];
        var allRewardedVideoAdUnits = new string[0];

        foreach (var bannerAdUnits in _bannerDict.Values) {
            allBannerAdUnits = allBannerAdUnits.Union (bannerAdUnits).ToArray ();
        }

        foreach (var interstitialAdUnits in _interstitialDict.Values) {
            allInterstitialAdUnits = allInterstitialAdUnits.Union (interstitialAdUnits).ToArray ();
        }

        foreach (var rewardedVideoAdUnits in _rewardedVideoDict.Values) {
            allRewardedVideoAdUnits = allRewardedVideoAdUnits.Union (rewardedVideoAdUnits).ToArray ();
        }

        foreach (var rewardedRichMediaAdUnits in _rewardedRichMediaDict.Values) {
            allRewardedVideoAdUnits = allRewardedVideoAdUnits.Union (rewardedRichMediaAdUnits).ToArray ();
        }

        addAdUnitsToStateMaps (allBannerAdUnits);
        addAdUnitsToStateMaps (allInterstitialAdUnits);
        addAdUnitsToStateMaps (allRewardedVideoAdUnits);

        #if UNITY_ANDROID && !UNITY_EDITOR
        MoPub.loadBannerPluginsForAdUnits (allBannerAdUnits);
        MoPub.loadInterstitialPluginsForAdUnits (allInterstitialAdUnits);
        MoPub.loadRewardedVideoPluginsForAdUnits (allRewardedVideoAdUnits);
        #elif UNITY_IPHONE && !UNITY_EDITOR
        MoPub.loadPluginsForAdUnits(allBannerAdUnits);
        MoPub.loadPluginsForAdUnits(allInterstitialAdUnits);
        MoPub.loadPluginsForAdUnits(allRewardedVideoAdUnits);
        #endif

        #if !UNITY_EDITOR
        if (!IsAdUnitArrayNullOrEmpty (allRewardedVideoAdUnits)) {
            MoPub.initializeRewardedVideo ();
        }
        #endif

        #if !(UNITY_ANDROID || UNITY_IPHONE)
        Debug.LogWarning("Please switch to either Android or iOS platforms to run sample app!");
        #endif
    }


    void OnGUI () {
        ConfigureGUI ();

        CreateNetworksTab ();

        GUILayout.BeginArea (new Rect (20, 0, Screen.width - 40, Screen.height));
        GUILayout.BeginVertical ();

        CreateTitleSection ();
        CreateBannersSection ();
        CreateInterstitialsSection ();
        List<MoPubMediationSetting> mediationSettings = GetMediationSettings ();
        CreateRewardedVideosSection (mediationSettings);
        CreateRewardedRichMediaSection (mediationSettings);
        CreateActionsSection ();

        GUILayout.EndVertical ();
        GUILayout.EndArea ();
    }


    private void ConfigureGUI () {
        // Set default label style
        GUI.skin.label.fontSize = 42;

        // Set default button style
        GUI.skin.button.margin = new RectOffset (0, 0, 10, 0);
        GUI.skin.button.stretchWidth = true;
        GUI.skin.button.fixedHeight = (Screen.width >= 960 || Screen.height >= 960) ? 75 : 50;
        GUI.skin.button.fontSize = 34;

        // Set default text field style
        GUI.skin.textField.stretchWidth = true;
        GUI.skin.textField.fixedHeight = 35;
        GUI.skin.textField.fontSize = 28;

        // Buffer space between sections
        _smallerFont = new GUIStyle (GUI.skin.label);
        _smallerFont.fontSize = GUI.skin.button.fontSize;

        _sectionMarginSize = GUI.skin.label.fontSize;
    }


    private void CreateNetworksTab () {
        _selectedToggleIndex = GUI.Toolbar (
            new Rect (0, Screen.height - GUI.skin.button.fixedHeight, Screen.width, GUI.skin.button.fixedHeight),
            _selectedToggleIndex,
            _networkList);
        _network = _networkList[_selectedToggleIndex];
        _bannerAdUnits = _bannerDict.ContainsKey (_network) ? _bannerDict[_network] : null;
        _interstitialAdUnits = _interstitialDict.ContainsKey (_network) ? _interstitialDict[_network] : null;
        _rewardedVideoAdUnits = _rewardedVideoDict.ContainsKey (_network) ? _rewardedVideoDict[_network] : null;
        _rewardedRichMediaAdUnits = _rewardedRichMediaDict.ContainsKey (_network) ? _rewardedRichMediaDict[_network] : null;
    }


    private void CreateTitleSection () {
        // App title including Plugin and SDK versions
        GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontSize = 48;
        GUI.Label (new Rect (0, 10, Screen.width, 60), MoPub.getPluginName (), centeredStyle);
        centeredStyle.fontSize = _smallerFont.fontSize;
        GUI.Label (new Rect (0, 70, Screen.width, 60), "with " + MoPub.getSDKName (), centeredStyle);
    }


    private void CreateBannersSection () {
        int titlePadding = 102;
        GUILayout.Space (titlePadding);
        GUILayout.Label ("Banners");
        if (!IsAdUnitArrayNullOrEmpty (_bannerAdUnits)) {
            foreach (string bannerAdUnit in _bannerAdUnits) {
                GUILayout.BeginHorizontal ();

                GUI.enabled = !_adUnitToLoadedMapping[bannerAdUnit];
                if (GUILayout.Button (CreateRequestButtonLabel (bannerAdUnit))) {
                    Debug.Log ("requesting banner with AdUnit: " + bannerAdUnit);
                    MoPub.createBanner (bannerAdUnit, MoPubAdPosition.BottomRight);
                    _requestedBannerAdUnits.Enqueue (bannerAdUnit);
                }

                GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit];
                if (GUILayout.Button ("Destroy")) {
                    MoPub.destroyBanner (bannerAdUnit);
                    _adUnitToLoadedMapping[bannerAdUnit] = false;
                    _bannerAdUnitToShownMapping[bannerAdUnit] = false;
                }

                GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit] && !_bannerAdUnitToShownMapping[bannerAdUnit];
                if (GUILayout.Button ("Show")) {
                    MoPub.showBanner (bannerAdUnit, true);
                    _bannerAdUnitToShownMapping[bannerAdUnit] = true;
                }

                GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit] && _bannerAdUnitToShownMapping[bannerAdUnit];
                if (GUILayout.Button ("Hide")) {
                    MoPub.showBanner (bannerAdUnit, false);
                    _bannerAdUnitToShownMapping[bannerAdUnit] = false;
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal ();
            }
        } else {
            GUILayout.Label ("No banner AdUnits for " + _network, _smallerFont, null);
        }
    }


    private void CreateInterstitialsSection () {
        GUILayout.Space (_sectionMarginSize);
        GUILayout.Label ("Interstitials");
        if (!IsAdUnitArrayNullOrEmpty (_interstitialAdUnits)) {
            foreach (string interstitialAdUnit in _interstitialAdUnits) {
                GUILayout.BeginHorizontal ();

                GUI.enabled = !_adUnitToLoadedMapping[interstitialAdUnit];
                if (GUILayout.Button (CreateRequestButtonLabel (interstitialAdUnit))) {
                    Debug.Log ("requesting interstitial with AdUnit: " + interstitialAdUnit);
                    MoPub.requestInterstitialAd (interstitialAdUnit);
                }

                GUI.enabled = _adUnitToLoadedMapping[interstitialAdUnit];
                if (GUILayout.Button ("Show")) {
                    MoPub.showInterstitialAd (interstitialAdUnit);
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal ();
            }
        } else {
            GUILayout.Label ("No interstitial AdUnits for " + _network, _smallerFont, null);
        }
    }


    private List<MoPubMediationSetting> GetMediationSettings () {
        List<MoPubMediationSetting> mediationSettings = new List<MoPubMediationSetting> ();

        #if UNITY_ANDROID
        MoPubMediationSetting adColonySettings = new MoPubMediationSetting ("AdColony");
        adColonySettings.Add ("withConfirmationDialog", true);
        adColonySettings.Add ("withResultsDialog", true);

        MoPubMediationSetting chartboostSettings = new MoPubMediationSetting ("Chartboost");
        chartboostSettings.Add ("customId", "the-user-id");

        MoPubMediationSetting vungleSettings = new MoPubMediationSetting ("Vungle");
        vungleSettings.Add ("userId", "the-user-id");
        vungleSettings.Add ("cancelDialogBody", "Cancel Body");
        vungleSettings.Add ("cancelDialogCloseButton", "Shut it Down");
        vungleSettings.Add ("cancelDialogKeepWatchingButton", "Watch On");
        vungleSettings.Add ("cancelDialogTitle", "Cancel Title");

        mediationSettings.Add (adColonySettings);
        mediationSettings.Add (chartboostSettings);
        mediationSettings.Add (vungleSettings);

        #elif UNITY_IPHONE
        MoPubMediationSetting adColonySettings = new MoPubMediationSetting ("AdColony");
        adColonySettings.Add ("showPrePopup", true);
        adColonySettings.Add ("showPostPopup", true);

        MoPubMediationSetting vungleSettings = new MoPubMediationSetting ("Vungle");
        vungleSettings.Add ("userIdentifier", "the-user-id");

        mediationSettings.Add (adColonySettings);
        mediationSettings.Add (vungleSettings);
        #endif

        return mediationSettings;
    }


    private void CreateRewardedVideosSection (List<MoPubMediationSetting> mediationSettings) {
        GUILayout.Space (_sectionMarginSize);
        GUILayout.Label ("Rewarded Videos");
        if (!IsAdUnitArrayNullOrEmpty (_rewardedVideoAdUnits)) {
            CreateCustomDataField ("rvCustomDataField", ref _rvCustomData);
            foreach (string rewardedVideoAdUnit in _rewardedVideoAdUnits) {
                GUILayout.BeginHorizontal ();

                GUI.enabled = !_adUnitToLoadedMapping[rewardedVideoAdUnit];
                if (GUILayout.Button (CreateRequestButtonLabel (rewardedVideoAdUnit))) {
                    Debug.Log ("requesting rewarded video with AdUnit: " +
                        rewardedVideoAdUnit +
                        " and mediation settings: " +
                        MoPubInternal.ThirdParty.MiniJSON.Json.Serialize (mediationSettings));
                    MoPub.requestRewardedVideo (rewardedVideoAdUnit,
                        mediationSettings,
                        "rewarded, video, mopub",
                        37.7833,
                        122.4167,
                        "customer101");
                }

                GUI.enabled = _adUnitToLoadedMapping[rewardedVideoAdUnit];
                if (GUILayout.Button ("Show")) {
                    MoPub.showRewardedVideo (rewardedVideoAdUnit, GetCustomData (_rvCustomData));
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal ();

                #if !UNITY_EDITOR
                // Display rewards if there's a rewarded video loaded and there are multiple rewards available
                if (MoPub.hasRewardedVideo (rewardedVideoAdUnit) &&
                    _adUnitToRewardsMapping.ContainsKey (rewardedVideoAdUnit) &&
                    _adUnitToRewardsMapping[rewardedVideoAdUnit].Count > 1) {

                    GUILayout.BeginVertical ();
                    GUILayout.Space (_sectionMarginSize);
                    GUILayout.Label ("Select a reward:");

                    foreach (MoPubReward reward in _adUnitToRewardsMapping[rewardedVideoAdUnit]) {
                        if (GUILayout.Button (reward.ToString ())) {
                            MoPub.selectReward (rewardedVideoAdUnit, reward);
                        }
                    }

                    GUILayout.Space (_sectionMarginSize);
                    GUILayout.EndVertical ();
                }
                #endif
            }
        } else {
            GUILayout.Label ("No rewarded video AdUnits for " + _network, _smallerFont, null);
        }
    }


    private void CreateRewardedRichMediaSection (List<MoPubMediationSetting> mediationSettings)
    {
        GUILayout.Space (_sectionMarginSize);
        GUILayout.Label ("Rewarded Rich Media");
        if (!IsAdUnitArrayNullOrEmpty (_rewardedRichMediaAdUnits)) {
            CreateCustomDataField ("rrmCustomDataField", ref _rrmCustomData);
            foreach (string rewardedRichMediaAdUnit in _rewardedRichMediaAdUnits) {
                GUILayout.BeginHorizontal ();

                GUI.enabled = !_adUnitToLoadedMapping[rewardedRichMediaAdUnit];
                if (GUILayout.Button (CreateRequestButtonLabel (rewardedRichMediaAdUnit))) {
                    Debug.Log ("requesting rewarded rich media with AdUnit: " +
                        rewardedRichMediaAdUnit +
                        " and mediation settings: " +
                        MoPubInternal.ThirdParty.MiniJSON.Json.Serialize (mediationSettings));
                    MoPub.requestRewardedVideo (rewardedRichMediaAdUnit,
                        mediationSettings,
                        "rewarded, video, mopub",
                        37.7833,
                        122.4167,
                        "customer101");
                }

                GUI.enabled = _adUnitToLoadedMapping[rewardedRichMediaAdUnit];
                if (GUILayout.Button ("Show")) {
                    MoPub.showRewardedVideo (rewardedRichMediaAdUnit, GetCustomData (_rrmCustomData));
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal ();

                #if !UNITY_EDITOR
                // Display rewards if there's a rewarded rich media ad loaded and there are multiple rewards available
                if (MoPub.hasRewardedVideo (rewardedRichMediaAdUnit) &&
                    _adUnitToRewardsMapping.ContainsKey (rewardedRichMediaAdUnit) &&
                    _adUnitToRewardsMapping[rewardedRichMediaAdUnit].Count > 1) {

                    GUILayout.BeginVertical ();
                    GUILayout.Space (_sectionMarginSize);
                    GUILayout.Label ("Select a reward:");

                    foreach (MoPubReward reward in _adUnitToRewardsMapping[rewardedRichMediaAdUnit]) {
                        if (GUILayout.Button (reward.ToString ())) {
                            MoPub.selectReward (rewardedRichMediaAdUnit, reward);
                        }
                    }

                    GUILayout.Space (_sectionMarginSize);
                    GUILayout.EndVertical ();
                }
                #endif
            }
        } else {
            GUILayout.Label ("No rewarded rich media AdUnits for " + _network, _smallerFont, null);
        }
    }


    private void CreateCustomDataField (string fieldName, ref string customDataValue)
    {
        GUI.SetNextControlName (fieldName);
        customDataValue = GUILayout.TextField (customDataValue, new GUILayoutOption[] { GUILayout.MinWidth(200) });
        if (UnityEngine.Event.current.type == EventType.Repaint) {
            if (GUI.GetNameOfFocusedControl () == fieldName && customDataValue == _customDataDefaultText) {
                // Clear default text when focused
                customDataValue = "";
            } else if (GUI.GetNameOfFocusedControl () != fieldName && customDataValue == "") {
                // Restore default text when unfocused and empty
                customDataValue = _customDataDefaultText;
            }
        }
    }


    private string GetCustomData (string customDataFieldValue)
    {
        return customDataFieldValue != _customDataDefaultText ? customDataFieldValue : null;
    }


    private void CreateActionsSection ()
    {
        GUILayout.Space (_sectionMarginSize);
        GUILayout.Label ("Actions");
        if (GUILayout.Button ("Report App Open")) {
            MoPub.reportApplicationOpen ();
        }
        if (GUILayout.Button ("Enable Location Support")) {
            MoPub.enableLocationSupport (true);
        }
    }


    private string CreateRequestButtonLabel (string adUnit) {
        return "Request " + adUnit.Substring (0, 10) + "...";
    }
}
