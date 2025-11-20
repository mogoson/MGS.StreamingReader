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
using UnityEngine;

namespace MGS.Streaming
{
    public sealed class MonoAvatar : MonoBehaviour, IDisposable
    {
        public static MonoAvatar CreateOne()
        {
            var go = new GameObject(nameof(MonoAvatar));
            DontDestroyOnLoad(go);
            return go.AddComponent<MonoAvatar>();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}