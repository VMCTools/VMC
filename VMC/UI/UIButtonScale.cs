#if VMC_DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace VMC.UI
{
    public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        private bool getOriginalScale = false;
        private Vector3 normalScale;
#if VMC_DOTWEEN
        private Tween tweenScale;
#endif
        private void OnEnable()
        {
            if (!getOriginalScale)
            {
                getOriginalScale = true;
                normalScale = transform.localScale;
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            if (tweenScale != null) tweenScale.Kill();
            tweenScale = transform.DOScale(normalScale * 0.9f, 0.2f);
#else
            transform.localScale = normalScale * 0.9f;
#endif
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            if (tweenScale != null) tweenScale.Kill();
            tweenScale = transform.DOScale(normalScale, 0.2f);
#else
            transform.localScale = normalScale;
#endif
        }
        public void OnPointerExit(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            if (tweenScale != null) tweenScale.Kill();
            tweenScale = transform.DOScale(normalScale, 0.2f);
#else
            transform.localScale = normalScale;
#endif
        }
    }
}
