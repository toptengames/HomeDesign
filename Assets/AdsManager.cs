using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEngine;


namespace ITSoft {
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] private string interId;
        [SerializeField] private string rewardedId;
        public static Action OnCompleteRewardVideo;
        public static Action OnCompleteInterVideo;

        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        
        private static bool removeAds = false;

        private static AdsManager instance;
        
        private void Awake()
        {
#if UNITY_EDITOR
            GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.coins, -(int)GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins));   
#endif
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            bool.TryParse(PlayerPrefs.GetString("addfreechk", "false"), out removeAds);

            Init();
            LoadInterstitial();
        }

        private void OnEnable()
        {
            interstitialAd.OnAdFailedToLoad += LoadInterstitial;
            interstitialAd.OnAdClosed += LoadInterstitial;
            interstitialAd.OnAdClosed += InterVideoAdRewardedEvent;
            rewardedAd.OnUserEarnedReward += RewardedVideoAdRewardedEvent;
            rewardedAd.OnAdFailedToShow += ErrorShowingReward;
            rewardedAd.OnAdClosed += ErrorShowingReward;
        }

        private void ErrorShowingReward(object sender, EventArgs e)
        {
            CreateAndLoadRewardAd();
        }

        private void LoadInterstitial(object sender, EventArgs e)
        {
            CreateAndLoadInterAd();
        }

        private void LoadInterstitial(object sender, AdFailedToLoadEventArgs e)
        {
            CreateAndLoadInterAd();
        }
        
        private void LoadInterstitial()
        {
            CreateAndLoadInterAd();
        }

        private void OnDisable()
        {
            interstitialAd.OnAdFailedToLoad -= LoadInterstitial;
            interstitialAd.OnAdClosed -= LoadInterstitial;
            interstitialAd.OnAdClosed -= InterVideoAdRewardedEvent;
            rewardedAd.OnUserEarnedReward -= RewardedVideoAdRewardedEvent;
            rewardedAd.OnAdFailedToShow -= ErrorShowingReward;
            rewardedAd.OnAdClosed -= ErrorShowingReward;
        }

        private void Init()
        {
            MobileAds.Initialize(initStatus => { });

            CreateAndLoadInterAd();
            CreateAndLoadRewardAd();
            //IronSource.Agent.validateIntegration();
            //IronSource.Agent.init(appKey);
        }

        private void CreateAndLoadInterAd()
        {
            interstitialAd = new InterstitialAd(interId);
            var request = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(request);
        }

        private void CreateAndLoadRewardAd()
        {
            rewardedAd = new RewardedAd(rewardedId);
            var request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
        }
        
        void RewardedVideoAdRewardedEvent(object sender, Reward args)
        {
            //Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
            OnCompleteRewardVideo?.Invoke();
            CreateAndLoadRewardAd();
        }

        void InterVideoAdRewardedEvent(object sender, EventArgs e)
        {
            OnCompleteInterVideo?.Invoke();
            OnCompleteInterVideo = null;
        }

        public static void RemoveAds()
        {
            removeAds = true;
        }
        
        public static bool RewardIsReady() => instance.rewardedAd.IsLoaded();
        public static bool InterIsReady() => instance.interstitialAd.IsLoaded();
        
        public static void ShowRewarded()
        {
            Debug.Log("Show reward video. Reward video is" + (RewardIsReady() ? "" : " not") + " ready");
            // if (removeAds)
            // {
            //     OnCompleteRewardVideo?.Invoke();
            //     return;
            // }
            if (RewardIsReady())
            {
                instance.rewardedAd.Show();
                //IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
                #if UNITY_EDITOR
                OnCompleteRewardVideo?.Invoke();
                #endif
            }
        }

        private void ErrorShowingReward(object sender, AdErrorEventArgs args)
        {
            CreateAndLoadRewardAd();
        }
        
        public static void ShowInterstitial(System.Action ViewComplete = null)
        {
            // if (BizzyBeeGames.IAPManager.Instance.IsProductPurchased("removeads"))
            //     return;
#if UNITY_EDITOR
            Debug.Log("Show Inter");
            ViewComplete?.Invoke();
            return;
#endif
            if (removeAds)
            {
                ViewComplete?.Invoke();
                return;
            }
            if (InterIsReady())
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - True");
                OnCompleteInterVideo = ViewComplete;
                instance.interstitialAd.Show();
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
                instance.LoadInterstitial();
                ViewComplete?.Invoke();
            }
        }
    }
}
