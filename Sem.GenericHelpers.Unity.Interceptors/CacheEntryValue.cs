namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class CacheEntryValue
    {
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ValidUntil { get; set; }

        public string LocalIdentifier { get; set; }

        public object Object { get; set; }

        public string AsString()
        {
            var sb = new StringBuilder();

            switch (this.Object.GetType().Name)
            {
                case "String[]":
                    return string.Join("; ", (string[])this.Object);

                default:
                    try
                    {
                        var serializer = new XmlSerializer(this.Object.GetType());
                        using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
                        {
                            serializer.Serialize(writer, this.Object);
                            writer.Flush();
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        return this.Object.ToString();
                    }

                    return sb.ToString();
            }
        }
    }
}