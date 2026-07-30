/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  PersistentIO.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/21/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using MGS.IO;
using UnityEngine;

namespace MGS.Streaming
{
    public sealed class PersistentIO
    {
        public static string GetFilePath(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }

        #region File Read Api.
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
        #endregion

        #region File Write Api(Unity Override) in WebGL is async.
        public static Exception WriteAllBytes(string fileName, byte[] bytes)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllBytes(filePath, bytes);
        }

        public static Exception WriteAllLines(string fileName, IEnumerable<string> contents)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllLines(filePath, contents);
        }

        public static Exception WriteAllText(string fileName, string contents)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllText(filePath, contents);
        }
        #endregion
    }
}