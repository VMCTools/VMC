using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VMC.Settings
{
    //[CreateAssetMenu(fileName = "VMC Settings", menuName ="VMC/VMC Settings")]
    public class VMCSetting : EditorWindow
    {
        // Add a menu item named "Do Something" to MyMenu in the menu bar.
        [MenuItem("VMC/Setting")]
        static void DoSomething()
        {
            Selection.activeObject = VMCSettingConfig.LoadData();
        }
    }
}