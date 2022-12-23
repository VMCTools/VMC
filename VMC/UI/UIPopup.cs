#if VMC_DOTWEEN
using DG.Tweening;
#endif
using System;
using UnityEngine;
using UnityEngine.UI;

namespace VMC.UI
{
    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private Image imgBlack;
        [SerializeField] private GameObject boundPopup;
#if VMC_DOTWEEN
        [SerializeField] private float timeShow = 0.3f;
        [SerializeField] private bool ignoreTimeScale = true;
#endif
        [SerializeField] private float fadeColor = 0.8f;
        [SerializeField] protected Button btnClose;

        protected bool isAnimated=false;

        public event Action OnShowComplete;
        private void Start()
        {
            if (btnClose != null)
            {
                btnClose.onClick.AddListener(() =>
                {
                    HideDialog();
                });
            }
        }
        public void ShowDialog(Action callbackShow = null)
        {
#if VMC_DOTWEEN
            isAnimated = true;
            this.gameObject.SetActive(true);
            if (imgBlack)
            {
                var color = imgBlack.color;
                color.a = 0f;
                imgBlack.color = color;
                imgBlack.DOFade(fadeColor, timeShow);
            }
            if (boundPopup)
            {
                boundPopup.transform.localScale = Vector3.zero;
                boundPopup.transform.DOScale(1, timeShow).SetUpdate(ignoreTimeScale).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    callbackShow?.Invoke();
                    isAnimated = false;
                });
            }
            else
            {
                callbackShow?.Invoke();
                isAnimated = false;
            }
#else
            this.gameObject.SetActive(true);
            callbackShow?.Invoke();
            isAnimated = false;
#endif
        }
        public void HideDialog(Action callbackHide = null)
        {
#if VMC_DOTWEEN
            isAnimated = true;
            if (imgBlack)
                imgBlack.DOFade(0, timeShow);
            if (boundPopup)
            {
                boundPopup.transform.localScale = Vector3.one;
                boundPopup.transform.DOScale(0, timeShow).SetUpdate(ignoreTimeScale).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    callbackHide?.Invoke();
                    isAnimated = false;
                });
            }
            else
            {
                this.gameObject.SetActive(false);
                callbackHide?.Invoke();
                isAnimated = false;
            }
#else
            this.gameObject.SetActive(false);
            callbackHide?.Invoke();
            isAnimated = false;
#endif
        }

        protected virtual void OnEnable()
        {
            this.ShowDialog();
        }
        protected virtual void OnDisable()
        {
            OnShowComplete?.Invoke();
        }
    }
}