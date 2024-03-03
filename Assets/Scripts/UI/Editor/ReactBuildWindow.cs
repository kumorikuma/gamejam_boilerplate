using UnityEditor;
using UnityEngine;

public static class ReactBuildWindow
{

    [MenuItem("React/Build")]
    public static void Build()
    {
        // Run "npm run build" in the subdirectory
        string subdirectoryPath = Application.dataPath + "/../react";
        ReactBuildLogWindow.ClearLog();
        ReactBuildPreprocessor.RunNpmBuild(subdirectoryPath);
    }
}