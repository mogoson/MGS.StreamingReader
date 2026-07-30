/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  StreamingIO.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/20/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections;
using UnityEngine;

#if !(UNITY_ANDROID || UNITY_WEBGL)
using System.Collections.Generic;
using MGS.IO;
#endif

namespace MGS.Streaming
{
    public sealed class StreamingIO
    {
        public static string GetFilePath(string fileName)
        {
            return $"{Application.streamingAssetsPath}/{fileName}";
        }

#if !(UNITY_ANDROID || UNITY_WEBGL)
        //Android: streamingAssetsPath in package, can not read by Local File System.
        //WebGL: streamingAssetsPath in server directory, can not read by Local File System.

        public static string ReadAllText(string fileName, out Exception error)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.ReadAllText(filePath, out error);
        }

        public static IEnumerable<string> ReadLines(string fileName, out Exception error)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.ReadLines(filePath, out error);
        }

        public static byte[] ReadAllBytes(string fileName, out Exception error)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.ReadAllBytes(filePath, out error);
        }
#endif
        public static void ReadAsync(string fileName, Action<byte[], string, Exception> finished)
        {
            var filePath = GetFilePath(fileName);
            WebUtility.RequestAsync(filePath, finished);
        }

        public static IEnumerator ReadRoutine(string fileName, Action<byte[], string, Exception> finished)
        {
            var filePath = GetFilePath(fileName);
            return WebUtility.RequestRoutine(filePath, finished);
        }
    }
}