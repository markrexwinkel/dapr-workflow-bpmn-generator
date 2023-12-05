using Rex.Bpmn.Abstractions.Model;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Rex.Bpmn.Abstractions;

public class BpmnModel
{
    private static readonly Lazy<XmlSerializer> _serializer = new(() => new XmlSerializer(typeof(Definitions)));

    public Definitions Definitions { get; set; }

    public static BpmnModel Load(string path)
    {
        using var fs = File.OpenRead(path);
        return Load(fs);
    }

    public static BpmnModel Load(Stream stream)
    {
        using var xmlReader = XmlReader.Create(stream);
        return new BpmnModel
        {
            Definitions = (Definitions)_serializer.Value.Deserialize(xmlReader)
        };
    }

    public static BpmnModel Load(TextReader reader)
    {
        using var xmlReader = XmlReader.Create(reader);
        return new BpmnModel
        {
            Definitions = (Definitions)_serializer.Value.Deserialize(xmlReader)
        };
    }

    public static BpmnModel Parse(string xml)
    {
        using var reader = new StringReader(xml);
        return Load(reader);
    }
}
