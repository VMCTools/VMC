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
#endif
        [SerializeField] private float fadeColor = 0.8f;
        [SerializeField] protected Button btnClose;

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
            this.gameObject.SetActive(true);
            imgBlack.color = new Color32(0, 0, 0, 0);
            imgBlack.DOFade(fadeColor, timeShow);

            boundPopup.transform.localScale = Vector3.zero;
            boundPopup.transform.DOScale(1, timeShow).SetEase(Ease.OutBack).OnComplete(() =>
            {
                callbackShow?.Invoke();
            });
#else
            this.gameObject.SetActive(true);
            imgBlack.color = new Color(0, 0, 0, fadeColor);
            boundPopup.transform.localScale = Vector3.one;
            callbackShow?.Invoke();
#endif
        }

        public void HideDialog(Action callbackHide = null)
        {
#if VMC_DOTWEEN
            imgBlack.DOFade(0, timeShow);
            boundPopup.transform.localScale = Vector3.one;
            boundPopup.transform.DOScale(0, timeShow).SetEase(Ease.InBack).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                callbackHide?.Invoke();
            });
#else
            imgBlack.color = new Color(0, 0, 0, 0);
            boundPopup.transform.localScale = Vector3.zero;
            this.gameObject.SetActive(false);
            callbackHide?.Invoke();
#endif
        }
    }
}