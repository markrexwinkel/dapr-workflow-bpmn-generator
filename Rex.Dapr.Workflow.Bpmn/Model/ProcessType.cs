﻿using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tProcessType", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    public enum ProcessType
    {
        None,
        Public,
        Private
    }
}