using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tResource", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("resource", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class Resource : RootElement
    {
        private readonly Lazy<Collection<ResourceParameter>> _resourceParameters = new Lazy<Collection<ResourceParameter>>();

        [XmlElement("resourceParameter")]
        public Collection<ResourceParameter> ResourceParameters => _resourceParameters.Value;

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}