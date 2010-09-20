﻿namespace Sem.GenericHelpers.Contracts.Configuration
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