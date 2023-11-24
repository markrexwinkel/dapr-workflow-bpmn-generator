using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions.Model
{
    [XmlType("tUserTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("userTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class UserTask : Task
    {
        private readonly Lazy<Collection<Rendering>> _renderings = new Lazy<Collection<Rendering>>();

        [XmlElement("rendering")]
        public Collection<Rendering> Renderings => _renderings.Value;

        [XmlAttribute("implementation")]
        [DefaultValue("##unspecified")]
        public string Implementation { get; set; } = "##unspecified";
    }
}