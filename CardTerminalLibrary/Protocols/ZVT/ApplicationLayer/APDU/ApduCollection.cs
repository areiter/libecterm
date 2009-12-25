using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class ApduCollection:List<IZvtApdu>
    {
        /// <summary>
        /// Looks for all apdus that are of Type T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FindApduOfType<T>() where T: class, IZvtApdu
        {
            List<T> apdus = new List<T>();

            foreach (IZvtApdu apdu in this)
            {
                if (apdu is T)
                    apdus.Add((T)apdu);            
            }

            return apdus.ToArray();

        }

        /// <summary>
        /// Finds the first APDU that is of type T, or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindFirstApduOfType<T>() where T : class, IZvtApdu
        {
            T[] apdus = FindApduOfType<T>();

            if (apdus == null || apdus.Length == 0)
                return null;

            return apdus[0];
        }
    }
}
