using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using VMC.Ultilities;

namespace VMC.IAP
{
    public abstract class IAPManagerAbs : Singleton<IAPManagerAbs>, IStoreListener
    {
        public IAPProduct[] products;

        [SerializeField]
        private bool isTest;
        public bool IsTest
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return isTest;
#endif
            }
        }

        private IStoreController m_StoreController;
        private IExtensionProvider m_StoreExtensionProvider;
        private IAppleExtensions m_AppleExtensions;
        private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

        public bool IsInitialized => m_StoreController != null && m_StoreExtensionProvider != null;


        public bool IsProcessing = false;

        private const float TimeToDelayHandledPending = 15f;
        private float countTimePend = -1;
        private Product pendingProduct;

        private void Start()
        {
            if (!IsInitialized)
            {
                this.InitializePurchasing();
            }
        }
        public void InitializePurchasing()
        {
            Debugger.Debug.Log("[IAP]", "Initializing!");
            var module = StandardPurchasingModule.Instance();
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(module, new IPurchasingModule[0]);
            for (int i = 0; i < products.Length; i++)
            {
                configurationBuilder.AddProduct(products[i].idPack, products[i].productType);
            }
            UnityPurchasing.Initialize(this, configurationBuilder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debugger.Debug.Log("[IAP]", "Initialized!");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
            RestorePurchase((value) => { });
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debugger.Debug.Log("[IAP]", "Initialize Failed! " + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            IsProcessing = false;
            Debugger.Debug.Log("[IAP]", $"[BuyProductFailed] {product.definition.id}. {failureReason}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            IsProcessing = false;
            if (BuySuccessedItem(purchaseEvent.purchasedProduct.definition.id))
            {
            }
            else
            {
                countTimePend = TimeToDelayHandledPending;
                pendingProduct = purchaseEvent.purchasedProduct;
                UnityEngine.Debug.Log("BuyProductID: SUCCESSED. but something wrong when pay the bonus to player!!!");
                return PurchaseProcessingResult.Pending;
            }

            return PurchaseProcessingResult.Complete;
        }
        private void Update()
        {
            if (!IsInitialized) return;

            if (pendingProduct != null)
            {
                countTimePend -= Time.deltaTime;
                if (countTimePend < 0)
                {
                    countTimePend = TimeToDelayHandledPending;
                    Debugger.Debug.Log("[IAP]", $" Try handling pending process!!!");
                    if (BuySuccessedItem(pendingProduct.definition.id))
                    {
                        m_StoreController.ConfirmPendingPurchase(pendingProduct);
                        pendingProduct = null;
                    }
                }
            }
        }
        public void BuyProductID(string productId)
        {
            if (IsTest)
            {
                BuySuccessedItem(productId);
                Debugger.Debug.Log("[IAP]", $"[BuyProductSuccessed] {productId}. TEST MODE is True");
                return;
            }
            if (this.IsInitialized && !IsProcessing)
            {
                Product product = m_StoreController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    IsProcessing = true;
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debugger.Debug.Log("[IAP]", $"[BuyProductFailed] {productId}. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debugger.Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        protected abstract bool BuySuccessedItem(string productId);

        public void RestorePurchase(Action<bool> callback)
        {
#if UNITY_IPHONE
            m_AppleExtensions.RestoreTransactions(callback);
#endif
#if UNITY_ANDROID
            m_GooglePlayStoreExtensions.RestoreTransactions(callback);
#endif
        }

        public IAPProduct GetProductById(string id)
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (products[i].idPack == id) return products[i];
            }
            return new IAPProduct();
        }
        public IAPProduct GetProductByType(ProductType type)
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (products[i].productType == type) return products[i];
            }
            return new IAPProduct();
        }

        public string GetPrice(string idPack)
        {
#if UNITY_EDITOR
            return GetProductById(idPack).price + "$";
#else
            if (m_StoreController == null) return "Error: 1";
            Product product = m_StoreController.products.WithID(idPack);
            if (product == null) return "Error: 2";
            return product.metadata.localizedPriceString;
#endif
        }
    }
}