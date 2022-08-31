using UnityEngine;
namespace VMC.UI
{
    public class UIFaceToCamera : MonoBehaviour
    {
        private Transform cameraTransform;
        private Transform myTransform;
        private void Awake()
        {
            myTransform = this.transform;
            cameraTransform = Camera.main.transform;
        }
        private void Update()
        {
            myTransform.rotation = cameraTransform.rotation;
        }
    }
}