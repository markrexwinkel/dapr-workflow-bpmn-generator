using Rex.Bpmn.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions
{
    public static class BaseElementExtensions
    {
        public static string GetExtensionAttribute(this IExtensible element, string name, string ns)
        {
            return element.ExtensionAttributes.FirstOrDefault(x => x.NamespaceURI == ns && x.LocalName == name)?.Value;
        }
        public static TExtension GetExtensionElement<TExtension>(this IExtensible baseElement)
        {
            var qn = GetQualifiedName(typeof(TExtension));
            var elem = baseElement?.ExtensionElements?.Elements.FirstOrDefault(x => x.LocalName == qn.Name && x.NamespaceURI == qn.Namespace);
            if (elem is not null)
            {
                var serializer = new XmlSerializer(typeof(TExtension));
                using var ms = new MemoryStream();
                var xmlWriter = XmlWriter.Create(ms);
                elem.WriteTo(xmlWriter);
                xmlWriter.Flush();
                ms.Position = 0;
                var xmlReader = XmlReader.Create(ms);
                return (TExtension)serializer.Deserialize(xmlReader);
            }
            return default;
        }

        public static IEnumerable<TExtension> GetExtensionElements<TExtension>(this IExtensible baseElement)
        {
            var qn = GetQualifiedName(typeof(TExtension));
            var elems = baseElement.ExtensionElements.Elements.Where(x => x.LocalName == qn.Name && x.NamespaceURI == qn.Namespace);
            var serializer = new XmlSerializer(typeof(TExtension));
            var ret = new List<TExtension>();
            foreach (var elem in elems)
            {
                using var ms = new MemoryStream();
                var xmlWriter = XmlWriter.Create(ms);
                elem.WriteTo(xmlWriter);
                xmlWriter.Flush();
                ms.Position = 0;
                var xmlReader = XmlReader.Create(ms);
                var obj = (TExtension)serializer.Deserialize(xmlReader);
                ret.Add(obj);
            }
            return ret;
        }

        public static IEnumerable<DaprParameter> GetDaprInputParameters(this IExtensible baseElement)
        {
            return baseElement?.GetExtensionElement<DaprInputOutput>()?.InputParameters ?? Enumerable.Empty<DaprParameter>();
        }

        public static IEnumerable<DaprParameter> GetDaprOutputParameters(this IExtensible baseElement)
        {
            return baseElement.GetExtensionElement<DaprInputOutput>()?.OutputParameters ?? Enumerable.Empty<DaprParameter>();
        }

        private static XmlQualifiedName GetQualifiedName(Type type)
        {
            var xmlRootAttr = type.GetCustomAttribute<XmlRootAttribute>();
            var xmlTypeAttr = type.GetCustomAttribute<XmlTypeAttribute>();
            var ns = xmlRootAttr?.Namespace ?? xmlTypeAttr?.Namespace ?? string.Empty;
            var name = xmlRootAttr?.ElementName ?? type.Name;
            return new XmlQualifiedName(name, ns);
        }
    }
}
