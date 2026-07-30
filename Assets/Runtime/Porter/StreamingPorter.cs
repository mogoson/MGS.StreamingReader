using System;
using System.Collections;
using MGS.Singleton;
using UnityEngine;

namespace MGS.Streaming
{
    public sealed class StreamingPorter
    {
        public const string FILE_MANIFEST = "manifest";
        const string DIR_TRANSPORT = "Transport";
        const string KEY_TRANSPORT_VERSION = "PORTER_TRANSPORT_VERSION";

        /// <summary>
        /// Transport assets from streamingAssetsPath to persistentDataPath.
        /// </summary>
        /// <param name="finished">Finished action<newVersion, error></param>
        public static void TransportAsync(Action<string, Exception> finished)
        {
            MonoSingleton.Instance.StartCoroutine(StartTransport(finished));
        }

        static IEnumerator StartTransport(Action<string, Exception> finished)
        {
            string[] manifest = null;
            Exception error = null;
            yield return LoadManifest((array, ex) => { manifest = array; error = ex; });
            if (manifest == null || manifest.Length < 2)
            {
                finished?.Invoke(null, error);
                yield break;
            }

            long.TryParse(manifest[0], out long manifestVersion);
            long.TryParse(GetTransportVersion(), out long tsptVersion);
            if (manifestVersion <= tsptVersion)
            {
                finished?.Invoke(null, null);
                yield break;
            }

            for (int i = 1; i < manifest.Length; i++)
            {
                var fileName = GetFileName(manifest[i]);
                yield return TransportFile(fileName, ex => error = ex);
                if (error != null)
                {
                    finished?.Invoke(null, error);
                    yield break;
                }
            }

            var newVersion = manifestVersion.ToString();
            SetTransportVersion(newVersion);
            finished?.Invoke(newVersion, null);
        }

        static IEnumerator LoadManifest(Action<string[], Exception> finished)
        {
            string manifest = null;
            Exception error = null;
            var manifestName = GetFileName(FILE_MANIFEST);
            yield return StreamingIO.ReadRoutine(manifestName, (data, text, ex) => { manifest = text; error = ex; });
            if (error != null)
            {
                finished?.Invoke(null, error);
                yield break;
            }
            finished?.Invoke(ParseManifest(manifest), null);
        }

        static string[] ParseManifest(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            return content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        }

        static IEnumerator TransportFile(string fileName, Action<Exception> finished)
        {
            byte[] bytes = null;
            string text = null;
            Exception error = null;
            yield return StreamingIO.ReadRoutine(fileName, (data, str, ex) => { bytes = data; text = str; error = ex; });
            if (error != null)
            {
                finished?.Invoke(error);
                yield break;
            }
            var isSucceed = WriteToFile(fileName, bytes, text);
            finished?.Invoke(isSucceed);
        }

        static Exception WriteToFile(string fileName, byte[] bytes, string text)
        {
            if (bytes != null)
            {
                return PersistentIO.WriteAllBytes(fileName, bytes);
            }
            if (!string.IsNullOrEmpty(text))
            {
                return PersistentIO.WriteAllText(fileName, text);
            }
            return null;
        }

        public static string GetFileName(string fileName)
        {
            return $"{DIR_TRANSPORT}/{fileName}";
        }

        public static string GetSourcePath(string fileName)
        {
            return StreamingIO.GetFilePath(GetFileName(fileName));
        }

        public static string GetDestPath(string fileName)
        {
            return PersistentIO.GetFilePath(GetFileName(fileName));
        }

        static string GetTransportVersion()
        {
            return PlayerPrefs.GetString(KEY_TRANSPORT_VERSION);
        }

        static void SetTransportVersion(string version)
        {
            PlayerPrefs.SetString(KEY_TRANSPORT_VERSION, version);
        }
    }
}