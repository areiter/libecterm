using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT
{
    public static class ResourceHelper
    {
        public static ResourceManager Resources = LangDe.ResourceManager;

        public static string GetResourceString(string environment, object identifier)
        {
            try
            {
                return Resources.GetString(environment + "_" + identifier.ToString());
            }
            catch (Exception)
            {
                return identifier.ToString();
            }
        }
    }
}
