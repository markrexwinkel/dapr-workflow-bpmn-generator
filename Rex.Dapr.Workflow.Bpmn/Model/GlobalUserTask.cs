using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tGlobalUserTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("globalUserTask", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class GlobalUserTask : GlobalTask
    {
        private readonly Lazy<Collection<Rendering>> _renderings = new Lazy<Collection<Rendering>>();

        [XmlElement("rendering")]
        public Collection<Rendering> Renderings => _renderings.Value;

        [XmlAttribute("implementation")]
        [DefaultValue("##unspecified")]
        public string Implementation { get; set; } = "##unspecified";
    }
}