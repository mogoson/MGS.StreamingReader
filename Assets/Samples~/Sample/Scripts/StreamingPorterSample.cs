/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  StreamingPorterSample.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/28/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MGS.Streaming.Sample
{
    public class StreamingPorterSample : MonoBehaviour
    {
        public Text text;

        void Start()
        {
#if UNITY_EDITOR
            var manifest = StreamingPorter.GetSourcePath(StreamingPorter.FILE_MANIFEST);
            if (!File.Exists(manifest))
            {
                var message = "Should config the StreamingPorter first!\r\n" +
                    "Create manifest by menu item 'Tools/Streaming Porter/Update',\r\n" +
                    "Move the files to 'StreamingAssets/Transport' directory those you want to transport to 'persistentDataPath' at runtime,\r\n" +
                    "Update manifest by menu item 'Tools/Streaming Porter/Update'.";
                text.text = message;
                text.color = Color.red;
                return;
            }
#endif
            text.text = "Wait streaming assets transport...";
            StreamingPorter.TransportAsync(OnFinished);
            void OnFinished(string newVersion, Exception error)
            {
                if (error != null)
                {
                    text.text = $"Transport streaming assets error: {error.Message}";
                    return;
                }

                if (!string.IsNullOrEmpty(newVersion))
                {
                    Debug.Log($"The porter have transported manifest assets from streaming to persistent path for version {newVersion}");
                }

                Destroy(gameObject);
            }
        }
    }
}