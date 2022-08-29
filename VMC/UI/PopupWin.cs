using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VMC.Ultilities;

namespace VMC.UI
{
    public class PopupWin : UIPopup
    {
        public enum WinAction
        {
            NextLevel,
            Replay,
            Home
        }
        private Action<WinAction> callback;

        [Header("Component " + nameof(PopupWin))]
        [SerializeField] private Button btnNextLevel;
        [SerializeField] private Button btnReplay;
        [SerializeField] private Button btnHome;
        private bool canInteract;
        public void ShowPopup(Action<WinAction> callback)
        {
            this.callback = callback;
            base.ShowDialog(OnShowSuccessed);
        }
        private void OnShowSuccessed()
        {
            canInteract = true;
            if (btnClose) btnClose.onClick.AddListener(OnBtnHomeClicked);
            if (btnHome) btnHome.onClick.SetListener(OnBtnHomeClicked);
            if (btnNextLevel) btnNextLevel.onClick.SetListener(OnBtnNextLevelClicked);
            if (btnReplay) btnReplay.onClick.SetListener(OnBtnReplayClicked);
        }

        private void OnBtnReplayClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(WinAction.Replay);
            base.HideDialog();
        }

        private void OnBtnNextLevelClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(WinAction.NextLevel);
            base.HideDialog();
        }
        private void OnBtnHomeClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(WinAction.Home);
            base.HideDialog();
        }
    }
}