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
    using System.Configuration;
    using System.Xml.Serialization;

    public class ConfigReader : ConfigurationSection
    {
        private static object current;
        private static Type currentType;
        private static readonly object Sync = new object();
        
        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            var serializer = new XmlSerializer(currentType);
            current = serializer.Deserialize(reader);
        }

        public static TResult GetConfig<TResult>() 
            where TResult : new()
        {
            lock (Sync)
            {
                if (current == null)
                {
                    currentType = typeof(TResult);
                    ConfigurationManager.GetSection(currentType.Name);
                }

                return (TResult)(current ?? new TResult());
            }
        }
    }
}