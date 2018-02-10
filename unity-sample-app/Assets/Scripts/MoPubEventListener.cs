using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MoPubEventListener : MonoBehaviour
{
	#if UNITY_ANDROID || UNITY_IPHONE

	private MoPubDemoGUI _demoGUI;

	void OnEnable ()
	{
		var type = typeof(MoPubDemoGUI);
		try {
			// first we see if the Demo GUI already exist in the scene
			_demoGUI = FindObjectOfType (type) as MoPubDemoGUI;
			if (_demoGUI == null) {
				Debug.LogWarning("MoPubDemoGUI not initialized.");
			}
		} catch (UnityException e) {
			Debug.LogException (e);
		}

		// Listen to all events for illustration purposes
		MoPubManager.onAdLoadedEvent += onAdLoadedEvent;
		MoPubManager.onAdFailedEvent += onAdFailedEvent;
		MoPubManager.onAdClickedEvent += onAdClickedEvent;
		MoPubManager.onAdExpandedEvent += onAdExpandedEvent;
		MoPubManager.onAdCollapsedEvent += onAdCollapsedEvent;

		MoPubManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MoPubManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
		MoPubManager.onInterstitialShownEvent += onInterstitialShownEvent;
		MoPubManager.onInterstitialClickedEvent += onInterstitialClickedEvent;
		MoPubManager.onInterstitialDismissedEvent += onInterstitialDismissedEvent;
		MoPubManager.onInterstitialExpiredEvent += onInterstitialExpiredEvent;

		MoPubManager.onRewardedVideoLoadedEvent += onRewardedVideoLoadedEvent;
		MoPubManager.onRewardedVideoFailedEvent += onRewardedVideoFailedEvent;
		MoPubManager.onRewardedVideoExpiredEvent += onRewardedVideoExpiredEvent;
		MoPubManager.onRewardedVideoShownEvent += onRewardedVideoShownEvent;
		MoPubManager.onRewardedVideoClickedEvent += onRewardedVideoClickedEvent;
		MoPubManager.onRewardedVideoFailedToPlayEvent += onRewardedVideoFailedToPlayEvent;
		MoPubManager.onRewardedVideoReceivedRewardEvent += onRewardedVideoReceivedRewardEvent;
		MoPubManager.onRewardedVideoClosedEvent += onRewardedVideoClosedEvent;
		MoPubManager.onRewardedVideoLeavingApplicationEvent += onRewardedVideoLeavingApplicationEvent;
	}


	void OnDisable ()
	{
		// Remove all event handlers
		MoPubManager.onAdLoadedEvent -= onAdLoadedEvent;
		MoPubManager.onAdFailedEvent -= onAdFailedEvent;
		MoPubManager.onAdClickedEvent -= onAdClickedEvent;
		MoPubManager.onAdExpandedEvent -= onAdExpandedEvent;
		MoPubManager.onAdCollapsedEvent -= onAdCollapsedEvent;

		MoPubManager.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
		MoPubManager.onInterstitialFailedEvent -= onInterstitialFailedEvent;
		MoPubManager.onInterstitialShownEvent -= onInterstitialShownEvent;
		MoPubManager.onInterstitialClickedEvent -= onInterstitialClickedEvent;
		MoPubManager.onInterstitialDismissedEvent -= onInterstitialDismissedEvent;
		MoPubManager.onInterstitialExpiredEvent -= onInterstitialExpiredEvent;

		MoPubManager.onRewardedVideoLoadedEvent -= onRewardedVideoLoadedEvent;
		MoPubManager.onRewardedVideoFailedEvent -= onRewardedVideoFailedEvent;
		MoPubManager.onRewardedVideoExpiredEvent -= onRewardedVideoExpiredEvent;
		MoPubManager.onRewardedVideoShownEvent -= onRewardedVideoShownEvent;
		MoPubManager.onRewardedVideoClickedEvent -= onRewardedVideoClickedEvent;
		MoPubManager.onRewardedVideoFailedToPlayEvent -= onRewardedVideoFailedToPlayEvent;
		MoPubManager.onRewardedVideoReceivedRewardEvent -= onRewardedVideoReceivedRewardEvent;
		MoPubManager.onRewardedVideoClosedEvent -= onRewardedVideoClosedEvent;
		MoPubManager.onRewardedVideoLeavingApplicationEvent -= onRewardedVideoLeavingApplicationEvent;
	}


	// Banner Events

	void onAdLoadedEvent (float height)
	{
		Debug.Log ("onAdLoadedEvent. height: " + height);
		_demoGUI.bannerLoaded ();
	}

	void onAdFailedEvent (string errorMsg)
	{
		Debug.Log ("onAdFailedEvent: " + errorMsg);
	}

	void onAdClickedEvent (string adUnitId)
	{
		Debug.Log ("onAdClickedEvent: " + adUnitId);
	}

	void onAdExpandedEvent (string adUnitId)
	{
		Debug.Log ("onAdExpandedEvent: " + adUnitId);
	}

	void onAdCollapsedEvent (string adUnitId)
	{
		Debug.Log ("onAdCollapsedEvent: " + adUnitId);
	}


	// Interstitial Events

	void onInterstitialLoadedEvent (string adUnitId)
	{
		Debug.Log ("onInterstitialLoadedEvent: " + adUnitId);
		_demoGUI.adLoaded (adUnitId);
	}

	void onInterstitialFailedEvent (string errorMsg)
	{
		Debug.Log ("onInterstitialFailedEvent: " + errorMsg);
	}

	void onInterstitialShownEvent (string adUnitId)
	{
		Debug.Log ("onInterstitialShownEvent: " + adUnitId);
	}

	void onInterstitialClickedEvent (string adUnitId)
	{
		Debug.Log ("onInterstitialClickedEvent: " + adUnitId);
	}

	void onInterstitialDismissedEvent (string adUnitId)
	{
		Debug.Log ("onInterstitialDismissedEvent: " + adUnitId);
		_demoGUI.adDismissed (adUnitId);
	}

	void onInterstitialExpiredEvent (string adUnitId)
	{
		Debug.Log ("onInterstitialExpiredEvent: " + adUnitId);
	}


	// Rewarded Video Events

	void onRewardedVideoLoadedEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoLoadedEvent: " + adUnitId);

		List<MoPubManager.MoPubReward> availableRewards = MoPub.getAVailableRewards (adUnitId);
		_demoGUI.adLoaded (adUnitId);
		_demoGUI.loadAvailableRewards (adUnitId, availableRewards);
	}

	void onRewardedVideoFailedEvent (string errorMsg)
	{
		Debug.Log ("onRewardedVideoFailedEvent: " + errorMsg);
	}

	void onRewardedVideoExpiredEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoExpiredEvent: " + adUnitId);
	}

	void onRewardedVideoShownEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoShownEvent: " + adUnitId);
	}

	void onRewardedVideoClickedEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoClickedEvent: " + adUnitId);
	}

	void onRewardedVideoFailedToPlayEvent (string errorMsg)
	{
		Debug.Log ("onRewardedVideoFailedToPlayEvent: " + errorMsg);
	}

	void onRewardedVideoReceivedRewardEvent (MoPubManager.RewardedVideoData rewardedVideoData)
	{
		Debug.Log ("onRewardedVideoReceivedRewardEvent: " + rewardedVideoData);
	}

	void onRewardedVideoClosedEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoClosedEvent: " + adUnitId);
		_demoGUI.adDismissed (adUnitId);
	}

	void onRewardedVideoLeavingApplicationEvent (string adUnitId)
	{
		Debug.Log ("onRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	#endif
}
