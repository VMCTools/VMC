using System;
using UnityEngine.Purchasing;

namespace VMC.IAP
{
    [Serializable]
    public struct IAPProduct
    {
        public string idPack;
        public ProductType productType;
        public float price;
        public float value;
    }
}