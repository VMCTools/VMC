using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

namespace VMC.Ultilities
{
    public static class MoveEffectExtensions
    {
        public static void LocalJumpTo(this Transform transform, Vector3 startPoint, Vector3 endPoint, float timeMove, Action callback = null)
        {
            float distance = Vector3.Distance(endPoint, startPoint);
            if (distance < 3) distance = 3;
            Vector3 middle = (startPoint + endPoint) / 2 + Vector3.up * (distance / 2);

            transform.DORotate(Vector3.one * 360, timeMove, RotateMode.FastBeyond360);
            transform.DOLocalPath(new Vector3[] { startPoint, middle, endPoint }, timeMove, PathType.CatmullRom).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        public static void WorldJumpTo(this Transform transform, Vector3 startPoint, Vector3 endPoint, float timeMove, Action callback = null)
        {
            float distance = Vector3.Distance(endPoint, startPoint);
            if (distance < 3) distance = 3;
            Vector3 middle = (startPoint + endPoint) / 2 + Vector3.up * (distance / 2);

            transform.DORotate(Vector3.one * 360, timeMove, RotateMode.FastBeyond360);
            transform.DOPath(new Vector3[] { startPoint, middle, endPoint }, timeMove, PathType.CatmullRom).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
    }
}