using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Caches
{
    /// <summary></summary>
    public class ObjectPlication : Cache
    {
        private static ObjectPlication objpli = new ObjectPlication();

        private ObjectPlication() { }

        /// <summary></summary>
        public static ObjectPlication getInstance() { return ObjectPlication.objpli; }

        /// <summary></summary>
        public bool IsAllowed(string key) { return this.Put(key, true); }

        /// <summary></summary>
        public void Leave(string key) { this.Remove(key); }
    }
}
