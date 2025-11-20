/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  WebUtility.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/20/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections;
using UnityEngine.Networking;

namespace MGS.Streaming
{
    public sealed class WebUtility
    {
        public static void GetWebDataAsync(string url, Action<byte[], string, Exception> finished)
        {
            var avatar = MonoAvatar.CreateOne();
            avatar.StartCoroutine(GetWebData(url, OnFinished));
            void OnFinished(byte[] data, string text, Exception error)
            {
                avatar.Dispose();
                finished.Invoke(data, text, error);
            }
        }

        public static IEnumerator GetWebData(string url, Action<byte[], string, Exception> finished)
        {
            using var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var data = request.downloadHandler.data;
                var text = request.downloadHandler.text;
                finished.Invoke(data, text, null);
                yield break;
            }
            var error = new Exception(request.error);
            finished.Invoke(null, null, error);
        }
    }
}