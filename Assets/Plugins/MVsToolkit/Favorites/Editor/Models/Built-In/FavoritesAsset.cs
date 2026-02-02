using System;
using Codice.CM.Common.Serialization.Replication;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public sealed class FavoritesAsset : IFavoritesElement
    {
        public FavoritesAsset(string itemGuid)
        {
            m_Guid = itemGuid;
        }
        public string Name => GetObject() != null ? GetObject().name : string.Empty;

        public override bool IsValid() => !string.IsNullOrEmpty(m_Guid);
        public override Object GetObject() => !IsValid() ? null : AssetDatabase.LoadAssetByGUID<Object>(new GUID(m_Guid));
    }
}