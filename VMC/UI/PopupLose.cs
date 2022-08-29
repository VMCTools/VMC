using System;
using UnityEngine;
using UnityEngine.UI;
using VMC.Ultilities;

namespace VMC.UI
{
    public class PopupLose : UIPopup
    {
        public enum LoseAction
        {
            Replay,
            Home
        }
        private Action<LoseAction> callback;

        [Header("Component " + nameof(PopupLose))]
        [SerializeField] private Button btnReplay;
        [SerializeField] private Button btnHome;
        private bool canInteract;
        public void ShowPopup(Action<LoseAction> callback)
        {
            this.callback = callback;
            base.ShowDialog(OnShowSuccessed);
        }
        private void OnShowSuccessed()
        {
            canInteract = true;
            if (btnClose) btnClose.onClick.AddListener(OnBtnHomeClicked);
            if (btnHome) btnHome.onClick.SetListener(OnBtnHomeClicked);
            if (btnReplay) btnReplay.onClick.SetListener(OnBtnReplayClicked);
        }

        private void OnBtnReplayClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(LoseAction.Replay);
            base.HideDialog();
        }
        private void OnBtnHomeClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(LoseAction.Home);
            base.HideDialog();
        }
    }
}
