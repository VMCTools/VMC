
#if VMC_ANALYZE_APPFLYER
using AppsFlyerSDK;
using System;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if VMC_IAP
using UnityEngine.Purchasing;
#endif
using UnityEngine.UI.Extensions;
using VMC.Settings;
using Debug = VMC.Debugger.Debug;

namespace VMC.Analystic
{
    public class AppsFlyerAnalystic : MonoBehaviour, IAnalystic
#if VMC_ANALYZE_APPFLYER
#if VMC_IAP
        , IStoreListener, IAppsFlyerValidateReceipt
#endif
        , IAppsFlyerConversionData
#endif
    {
        [SerializeField, ReadOnly] private string AF_Dev_Key;
        [SerializeField, ReadOnly] private string AF_App_Id;


        private const string PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0rv73WA6ePMXWpcxUlWrqvYycwOsdv90CVqBBCH8EMvlukFTnHB7FdhuGA1SPUU2b7aBRfbMahCvd7Z/81vd3lhcjHOyg194OgsBYhS+KsyWKem9w8Sd7tKfk4ubaCEmKgj2w0z+Us2eClJDUWh9AJZGq79HgpAwGSQclKZEGfAfSoZGrYSJc89perHH9OtlAbWeCRLsM9qMCVe3zTrRdDZ+1KAHezukxjKJTt6jAOvUrUYZ4+eBFP8oC4ssXSKroQTiGXwbzBJd6iQ9nPqHyo3g9Ro6VJ2g0OhYNMXrTryBJyLuYNnTVo+w4wZzeIZe/M+NHywwVY34u6lnKDxnhQIDAQAB"; // brickdoom


        public void Initialize()
        {
            Debug.Log("[Analystic]", "Init AppsFlyer!");
#if VMC_ANALYZE_APPFLYER
            VMC.Settings.VMCSettingConfig config = VMC.Settings.VMCSettingConfig.LoadData();
            AF_Dev_Key = config.AF_Dev_Key;
            AF_App_Id = config.AF_App_Id;

            AppsFlyer.initSDK(AF_Dev_Key, AF_App_Id, this);
            AppsFlyer.startSDK();
#endif
        }
        public void LogEvent(string nameEvent)
        {
            // Not log event
        }

        public void LogEvent(string nameEvent, Dictionary<string, string> param)
        {
            // Not log event
        }

        public void LogEvent(string eventName, AnalyzeLibrary specialPlatform)
        {
#if VMC_ANALYZE_APPFLYER
            if (!specialPlatform.HasFlag(AnalyzeLibrary.AppsFlyer))
            {
                return; // không chỉ định appflyer log event
            }
            AppsFlyer.sendEvent(eventName, null);
            Debug.Log("AppsFlyer", "Log message: " + eventName);
#endif
        }

#if VMC_ANALYZE_APPFLYER
        public void onAppOpenAttribution(string attributionData)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {
        }

        public void onConversionDataFail(string error)
        {
        }

        public void onConversionDataSuccess(string conversionData)
        {
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            string prodID = args.purchasedProduct.definition.id;
            string price = args.purchasedProduct.metadata.localizedPrice.ToString();
            string currency = args.purchasedProduct.metadata.isoCurrencyCode;

            Product product = args.purchasedProduct;
            string receipt = args.purchasedProduct.receipt;
            var recptToJSON = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize(product.receipt);
            var receiptPayload = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize((string)recptToJSON["Payload"]);
            var transactionID = product.transactionID;

            //if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
            {
#if UNITY_IOS
                if(isSandbox)
                {
                    AppsFlyeriOS.setUseReceiptValidationSandbox(true);
                }

                AppsFlyeriOS.validateAndSendInAppPurchase(prodID, price, currency, transactionID, null, this);
#elif UNITY_ANDROID
                var purchaseData = (string)receiptPayload["json"];
                var signature = (string)receiptPayload["signature"];
                new AppsFlyerAndroid().validateAndSendInAppPurchase(PublicKey, signature, purchaseData, price, currency, null, this);
#endif
            }

            return PurchaseProcessingResult.Complete;
        }

        public void didFinishValidateReceipt(string result)
        {
            AppsFlyer.AFLog("didFinishValidateReceipt", result);
        }

        public void didFinishValidateReceiptWithError(string error)
        {
            AppsFlyer.AFLog("didFinishValidateReceiptWithError", error);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
        }

#endif
    }
}