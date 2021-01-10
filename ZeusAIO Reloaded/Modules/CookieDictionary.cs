using Leaf.xNet;
using System.Net;

namespace TunnelBear
{
    internal class CookieDictionary : CookieStorage
    {
        public CookieDictionary(bool isLocked = false, CookieContainer container = null) : base(isLocked, container)
        {
        }
    }
}