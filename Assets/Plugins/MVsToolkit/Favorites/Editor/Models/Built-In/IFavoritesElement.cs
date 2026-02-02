using System;
using Object = UnityEngine.Object;

namespace MVsToolkit.Favorites
{
    [Serializable]
    public class IFavoritesElement
    {
        protected string m_Guid;
        public virtual bool IsValid(){return true;}
        public virtual Object GetObject(){return null;}
    }
}