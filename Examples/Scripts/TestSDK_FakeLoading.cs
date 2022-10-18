using UnityEngine;
using UnityEngine.UI;

namespace VMC.Examples
{
    public class TestSDK_FakeLoading : MonoBehaviour
    {

        [SerializeField] private VMCFirstLoading firstLoading;
        [SerializeField] private Text txtLoading;
        private void OnEnable()
        {
            firstLoading.OnProgressLoading += FirstLoading_OnProgressLoading;
        }
        private void OnDisable()
        {
            firstLoading.OnProgressLoading -= FirstLoading_OnProgressLoading;
        }
        private void FirstLoading_OnProgressLoading(float ratio)
        {
            txtLoading.text = $"Loading {Mathf.RoundToInt(ratio * 100)}%...";
        }
    }
}