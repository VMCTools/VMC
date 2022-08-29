using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VMC.Ultilities;

namespace VMC.UI
{
    public class PopupNotice : UIPopup
    {
        private Action callback;

        [Header("Component " + nameof(PopupNotice))]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private Button btnConfirm;
        private bool canInteract;
        public void ShowPopup(string title, string description, Action callback)
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
            if (btnClose) btnClose.onClick.SetListener(OnBtnConfirmClicked);
            if (btnConfirm) btnConfirm.onClick.SetListener(OnBtnConfirmClicked);
        }
        private void OnBtnConfirmClicked()
        {
            if (!canInteract) return;
            canInteract = false;
            callback?.Invoke();
            base.HideDialog();
        }
    }
}
