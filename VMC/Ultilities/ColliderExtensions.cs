using UnityEngine;

namespace VMC.Ultilities
{
    public static class ColliderExtensions
    {
        public static Vector3 Center(this Collider collider)
        {
            if (collider is SphereCollider sphereCollider)
            {
                return sphereCollider.center;
            }
            if (collider is BoxCollider boxCollider)
            {
                return boxCollider.center;
            }
            if (collider is CapsuleCollider capsuleCollider)
            {
                return capsuleCollider.center;
            }
            if (collider is WheelCollider wheelCollider)
            {
                return wheelCollider.center;
            }
            return Vector3.zero;
        }
    }
}