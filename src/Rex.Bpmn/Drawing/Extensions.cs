using Rex.Bpmn.Abstractions.Model;
using System.Xml;
using System.Xml.Linq;

namespace Rex.Bpmn.Drawing;

public static class Extensions
{
    public static XDocument ToSvgDocument(this BpmnDiagram diagram, Definitions definitions, IDictionary<string, int> tokens = null)
    {
        var visitor = new DiagramToSvgVisitor(definitions);
        return visitor.CreateSvgDiagram(diagram, tokens);
    }

    public static void WriteSvg(this BpmnDiagram diagram, Definitions definitions, Stream output, IDictionary<string, int> tokens = null)
    {
        var doc = diagram.ToSvgDocument(definitions, tokens);
        using var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true });
        doc.WriteTo(writer);
        writer.Flush();
    }

    public static void WriteSvg(this BpmnDiagram diagram, Definitions definitions, string filename, IDictionary<string, int> tokens = null)
    {
        var doc = diagram.ToSvgDocument(definitions, tokens);
        using var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true });
        doc.WriteTo(writer);
        writer.Flush();
    }
}
