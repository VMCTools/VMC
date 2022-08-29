using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VMC.Ultilities;

namespace VMC.UI
{
    public class PopupConfirm : UIPopup
    {
        public enum ConfirmOption
        {
            Yes,
            No
        }
        private Action<ConfirmOption> callback;

        [Header("Component " + nameof(PopupConfirm))]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private Button btnYes;
        [SerializeField] private Button btnNo;
        private bool canInteract;
        public void ShowPopup(string title, string description, Action<ConfirmOption> callback)
        {
            if (this.txtTitle) this.txtTitle.text = title;
            if (this.txtDescription) this.txtDescription.text = description;
            this.callback = callback;
            canInteract = false;
            base.ShowDialog(OnShowSuccessed);
        }
        private void OnShowSuccessed()
        {
            canInteract = true;
            if (btnClose) btnClose.onClick.SetListener(OnBtnNoClicked);
            if (btnYes) btnYes.onClick.SetListener(OnBtnYesClicked);
            if (btnNo) btnNo.onClick.SetListener(OnBtnNoClicked);
        }
        private void OnBtnYesClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(ConfirmOption.Yes);
            base.HideDialog();
        }
        private void OnBtnNoClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke(ConfirmOption.No);
            base.HideDialog();
        }
    }
}