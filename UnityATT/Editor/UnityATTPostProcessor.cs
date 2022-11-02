#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public static class UnityATTPostProcessor
{
    const string TrackingDescription =
        "This identifier will be used to deliver personalized ads to you. ";

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = PBXProject.GetPBXProjectPath(buildPath);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string targetName = PBXProject.GetUnityTargetName(); // note, not "project." ...
            string targetGUID = project.TargetGuidByName(targetName);

            AddFrameworks(project, targetGUID);
            // Write.
            File.WriteAllText(projectPath, project.WriteToString());

            AddPlistValues(buildPath);
        }
    }

    static void AddPlistValues(string pathToXcode)
    {
        // Get Plist from Xcode project 
        string plistPath = pathToXcode + "/Info.plist";

        // Read in Plist 
        PlistDocument plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));

        // set values from the root obj
        PlistElementDict plistRoot = plistObj.root;

        // Set value in plist
        plistRoot.SetString("NSUserTrackingUsageDescription", TrackingDescription);

        // save
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }

    static void AddFrameworks(PBXProject project, string targetGUID)
    {
        // Frameworks (eppz! Photos, Google Analytics).
        project.AddFrameworkToProject(targetGUID, "AppTrackingTransparency.framework", false);

        // Add `-ObjC` to "Other Linker Flags".
        project.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
    }

}
#endif