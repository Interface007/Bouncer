// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfigReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml.Serialization;

    public class ConfigReader : ConfigurationSection
    {
        private static readonly object Sync = new object();
        private static readonly Dictionary<Type, object> current = new Dictionary<Type, object>();
        private static Type currentType;
        
        public static TResult GetConfig<TResult>() 
            where TResult : class, new()
        {
            lock (Sync)
            {
                currentType = typeof(TResult);
                if (!current.ContainsKey(currentType))
                {
                    ConfigurationManager.GetSection(currentType.Name);
                    if (!current.ContainsKey(currentType))
                    {
                        current.Add(currentType, new TResult());
                    }
                }

                return (TResult)current[currentType];
            }
        }
     
        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            var serializer = new XmlSerializer(currentType);
            current.Add(currentType, serializer.Deserialize(reader));
        }
    }
}