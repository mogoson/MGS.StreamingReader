/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  PersistentReader.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  11/21/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using MGS.IOUtility;
using UnityEngine;

namespace MGS.Streaming
{
    public sealed class PersistentReader
    {
        public static string GetFilePath(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }

        #region File Write api(Unity Override) in WebGL is async.
        public static bool WriteAllBytes(string fileName, byte[] bytes)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllBytes(filePath, bytes);
        }

        public static bool WriteAllLines(string fileName, IEnumerable<string> contents)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllLines(filePath, contents);
        }

        public static bool WriteAllText(string fileName, string contents)
        {
            var filePath = GetFilePath(fileName);
            return FileUtility.WriteAllText(filePath, contents);
        }
        #endregion
    }
}