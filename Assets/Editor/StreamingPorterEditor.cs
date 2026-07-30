/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  StreamingPorterEditor.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/22/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using MGS.IO;
using UnityEditor;
using UnityEngine;

namespace MGS.Streaming.Editor
{
    public class StreamingPorterEditor
    {
        [MenuItem("Tools/Streaming Porter/Open/Source")]
        static void OpenSourcePath()
        {
            var sourcePath = StreamingPorter.GetSourcePath(string.Empty);
            Application.OpenURL(sourcePath);
        }

        [MenuItem("Tools/Streaming Porter/Open/Dest")]
        static void OpenDestPath()
        {
            var destPath = StreamingPorter.GetDestPath(string.Empty);
            Application.OpenURL(destPath);
        }

        [MenuItem("Tools/Streaming Porter/Update")]
        static void UpdateManifest()
        {
            var sourceDir = StreamingPorter.GetSourcePath(string.Empty);
            DirectoryUtility.Require(sourceDir);

            var lines = new List<string>() { DateTime.Now.ToFileTimeUtc().ToString() };
            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta") || file.EndsWith(StreamingPorter.FILE_MANIFEST))
                {
                    continue;
                }
                var name = file.Substring(sourceDir.Length);
                lines.Add(name);
            }

            var manifestFile = StreamingPorter.GetSourcePath(StreamingPorter.FILE_MANIFEST);
            var error = FileUtility.WriteAllLines(manifestFile, lines);
            if (error != null)
            {
                Debug.LogError($"Update manifest error: {error.Message}");
                return;
            }

            AssetDatabase.Refresh();
            Debug.Log($"The manifest of porter is updated to version {lines[0]}");
        }
    }
}