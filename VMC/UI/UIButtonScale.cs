#if VMC_DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace VMC.UI
{
    public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        private Vector3 normalScale;
        private void OnEnable()
        {
            normalScale = transform.localScale;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            transform.DOScale(normalScale * 0.9f, 0.2f);
#else
            transform.localScale = normalScale * 0.9f;
#endif
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            transform.DOScale(normalScale, 0.2f);
#else
            transform.localScale = normalScale;
#endif
        }
        public void OnPointerExit(PointerEventData eventData)
        {
#if VMC_DOTWEEN
            transform.DOScale(normalScale, 0.2f);
#else
            transform.localScale = normalScale;
#endif
        }
    }
}
