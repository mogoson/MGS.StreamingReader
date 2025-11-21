/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MonoAvatar.cs
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

namespace MGS.Streaming
{
    /// <summary>
    /// Avatar of MonoBehaviour for casual use.
    /// (Should hold the instance created by CreateOne method and dispose it if no longer needed.)
    /// </summary>
    public sealed class MonoAvatar : MonoBehaviour, IDisposable
    {
        public static MonoAvatar CreateOne()
        {
            var avatar = new GameObject(nameof(MonoAvatar)).AddComponent<MonoAvatar>();
            DontDestroyOnLoad(avatar.gameObject);
            return avatar;
        }

        public static void WaitForRoutine(IEnumerator routine)
        {
            var avatar = CreateOne();
            avatar.StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return routine;
                avatar.Dispose();
            }
        }

        public static void WaitForSeconds(float seconds, Action finished)
        {
            var avatar = CreateOne();
            avatar.StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(seconds);
                avatar.Dispose();
                finished?.Invoke();
            }
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}