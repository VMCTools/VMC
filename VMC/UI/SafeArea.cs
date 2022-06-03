using UnityEngine;

namespace VMC.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform panel;
        private Rect lastSafeArea = new Rect();

        private bool needUpdateFreequently = false;
        private void Awake()
        {

            panel = GetComponent<RectTransform>();


            Refresh();

            var orientation = Screen.orientation;
            switch (orientation)
            {
                case ScreenOrientation.AutoRotation:
                case ScreenOrientation.Landscape:
                case ScreenOrientation.Portrait:
                    needUpdateFreequently = true;
                    break;
            }
#if UNITY_EDITOR
            needUpdateFreequently = true;
#endif
        }

        private void Update()
        {
            if (needUpdateFreequently)
                Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = Screen.safeArea;
            if (safeArea != lastSafeArea)
                ApplySafeArea(safeArea);
        }

        private void ApplySafeArea(Rect r)
        {
            lastSafeArea = r;
            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;

            //Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            //    name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
        }
    }
}