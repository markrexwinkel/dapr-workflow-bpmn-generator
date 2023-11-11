using System.Xml.Serialization;

namespace Rex.Bpmn.Model
{
    [XmlType("tDataInputAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    [XmlRoot("dataInputAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class DataInputAssociation : DataAssociation
    {
    }
}