using UnityEngine;
using VMC.Ads;

public class SafeAreaWithBanner : MonoBehaviour
{
    private RectTransform _safeAreaRect;
    private Canvas _canvas;
    private Vector2 _canvasSize;
    private Rect _lastSafeArea;
    [Header("Khoảng cách các lề")]
    [SerializeField] private float bottomBannerShift = 0;
    [SerializeField] private float topBannerShift = 0;

    #region Unity Method

    void Awake()
    {
        _safeAreaRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        OnRectTransformDimensionsChange();
    }

    void Start()
    {
        _canvasSize = _canvas.GetComponent<RectTransform>().sizeDelta;
        AdsManager.OnBannerChange += UpdateBanner;
        UpdateBanner();
    }

    private void OnDestroy()
    {
        AdsManager.OnBannerChange -= UpdateBanner;
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateSizeToSafeArea();
    }

    #endregion

    private void UpdateSizeToSafeArea(Rect safeArea)
    {
        _safeAreaRect.anchoredPosition = Vector2.zero;
        _safeAreaRect.sizeDelta = Vector2.zero;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;

        if (AdsManager.Instance != null && !AdsManager.Instance.IsShowBannerBottom)
        {
            anchorMin.y = 0; //Fix bottom home button ios
        }
        else
        {
            anchorMin.y /= Screen.height;
        }

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        _safeAreaRect.anchorMin = anchorMin;
        _safeAreaRect.anchorMax = anchorMax;
    }

    private void UpdateBanner()
    {
        UpdateSizeToSafeArea(true);

        if (AdsManager.Instance.IsShowBannerBottom)
        {
            _safeAreaRect.offsetMin = new Vector2(_safeAreaRect.offsetMin.x,
                //RemoteConfig.instance.BannerAd_BottomSize
                ConvertDPToPixel(AdsManager.Instance.GetBannerHeight(), true)
                + bottomBannerShift);
        }
        else if (AdsManager.Instance.IsShowBannerTop)
        {
            _safeAreaRect.offsetMax = new Vector2(_safeAreaRect.offsetMax.x,
                -//RemoteConfig.instance.BannerAd_TopSize
                ConvertDPToPixel(AdsManager.Instance.GetBannerHeight(), false)
                + topBannerShift);
        }
        else
        {
            _safeAreaRect.offsetMin = new Vector2(_safeAreaRect.offsetMin.x, 0);
        }
    }

    private void UpdateSizeToSafeArea(bool isForce = false)
    {
        Rect safeArea = Screen.safeArea;
        if (_canvas != null && (isForce || safeArea != _lastSafeArea))
        {
            _lastSafeArea = safeArea;
            UpdateSizeToSafeArea(safeArea);
        }
    }

    public float ConvertDPToPixel(float dp, bool isBottom)
    {
#if UNITY_ANDROID
        float bannerSizePixels = dp * (Screen.dpi / 160);
#elif UNITY_IOS
        float bannerSizePixels = dp * (IOSDPI.dpi / 160); 
#else
        float bannerSizePixels = dp; 
#endif
        //VMC.Debugger.Debug.Log($"[Banner] pixel: {bannerSizePixels}");
        if (Display.main.systemHeight == Display.main.renderingHeight)
        {
            Rect safeArea = Screen.safeArea;
            float offsetWithSafeArea = (isBottom ? safeArea.y : (Screen.height - (safeArea.y + safeArea.height)));
            bannerSizePixels -= offsetWithSafeArea;
            //VMC.Debugger.Debug.Log($"[Banner] pixel fixed: {bannerSizePixels}");
        }
        float bannerSizeCanvas = bannerSizePixels / Screen.height * _canvasSize.y;
        //VMC.Debugger.Debug.Log($"[Banner] incanvas: {bannerSizeCanvas}");
        return bannerSizeCanvas;
    }
}
