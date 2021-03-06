using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Default object registry.
    /// </summary>
    public sealed class DefaultObjectRegistry : ObjectRegistryBase
    {
        private static volatile DefaultObjectRegistry _instance;
        private static readonly object Locker = new object();
        
        private DefaultObjectRegistry()
        {
            Tree = new ObjectTree(LoadDefaultModules());
        }

        /// <summary>
        /// Default instance.
        /// </summary>
        public static DefaultObjectRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultObjectRegistry();
                        }
                    }
                }
                
                return _instance;
            }
        }

        private static IList<ModuleLoader> LoadDefaultModules()
        {
            IList<ModuleLoader> result = new List<ModuleLoader>(5)
                                             {
                                                 LoadSingle(Resource.SNMPV2_SMI, "SNMPV2-SMI"),
                                                 LoadSingle(Resource.SNMPV2_CONF, "SNMPV2-CONF"),
                                                 LoadSingle(Resource.SNMPV2_TC, "SNMPV2-TC"),
                                                 LoadSingle(Resource.SNMPV2_MIB, "SNMPV2-MIB"),
                                                 LoadSingle(Resource.SNMPV2_TM, "SNMPV2-TM")
                                             };
            return result;
        }

        private static ModuleLoader LoadSingle(string mibFileContent, string name)
        {
            ModuleLoader result;
            using (TextReader reader = new StringReader(mibFileContent))
            {
                result = new ModuleLoader(reader, name);
            }
            
            return result;
        }
    }
}