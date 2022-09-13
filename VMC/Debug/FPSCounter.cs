using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VMC.Debugger
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        private TextMeshProUGUI _fpsText;

        private void Awake()
        {
            _fpsText = GetComponent<TextMeshProUGUI>();
        }
        private void Start()
        {
            //Below is slightly more solid (Just being nitpicky, sorry)
            InvokeRepeating(nameof(GetFPS), 1, 1);
        }

        private void GetFPS()
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
        }
    }
}