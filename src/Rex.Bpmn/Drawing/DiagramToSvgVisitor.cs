using Rex.Bpmn.Abstractions;
using Rex.Bpmn.Abstractions.Model;
using SixLabors.Fonts;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Rex.Bpmn.Drawing.StringHelpers;

namespace Rex.Bpmn.Drawing;

public partial class DiagramToSvgVisitor(Definitions definitions) : BpmnModelVisitor
{
    private const string SvgNamespace = "http://www.w3.org/2000/svg";
    private const string BiColorNamespace = "http://bpmn.io/schema/bpmn/biocolor/1.0";

    private const string SvgVersion = "1.1";

    private const float InnerOuterDistance = 3f;
    private const float TaskBorderRadius = 10f;
    private const string DefaultStrokeColor = "black";
    private const string DefaultFillColor = "white";

    private readonly Definitions _definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
    private readonly ElementFinder _finder = new();
    private XDocument _document;
    private XElement _root;
    private XElement _currentGroup;
    private XElement _defs;
    private IDictionary<string, int> _tokens;

    public XDocument CreateSvgDiagram(BpmnDiagram diagram, IDictionary<string, int> tokens = null)
    {
        _tokens = tokens ?? new Dictionary<string, int>();
        _document = new XDocument();
        (var width, var height) = CalculateWidthAndHeight(diagram.Plane);
        _currentGroup = new XElement(XName.Get("g", SvgNamespace));
        _root = new XElement(XName.Get("svg", SvgNamespace),
            new XAttribute("width", width),
            new XAttribute("height", height),
            new XAttribute("version", SvgVersion),
            _currentGroup);
        _document.Add(_root);
        _defs = new XElement(XName.Get("defs", SvgNamespace));
        _root.Add(_defs);
        AddMarkers();
        VisitDiagram(diagram);
        return _document;
    }

    private void AddMarkers()
    {
        _defs.Add(CreateMarker("sequenceflow-end", CreatePath("M 1 5 L 11 10 L 1 15 Z", "black"), 11f, 10f, 0.5f));
        _defs.Add(CreateMarker("messageflow-start", CreateCircle(6f, 6f, 3.5f), 6f, 6f));
        _defs.Add(CreateMarker("messageflow-end", CreatePath("m 1 5 l 0 -3 l 7 3 l -7 3 z"), 8.5f, 5f, strokeLineCap: "but"));
        _defs.Add(CreateMarker("association-start", CreatePath("M 11 5 L 1 10 L 11 15"), 1f, 10f, 0.5f, strokeWidth: 1.5f));
        _defs.Add(CreateMarker("association-end", CreatePath("M 1 5 L 11 10 L 1 15"), 12f, 10f, 0.5f, strokeWidth: 1.5f));
        _defs.Add(CreateMarker("conditional-flow-marker", CreatePath("M 0 10 L 8 6 L 16 10 L 8 14 Z", strokeWidth: 1f), -1f, 10f, 0.5f));
        _defs.Add(CreateMarker("conditional-default-flow-marker", CreatePath("M 6 4 L 10 16", strokeWidth: 1f), 0f, 10f, 0.5f));
    }

    private static XElement CreateMarker(string id, XElement element, float refX, float refY, float scale = 1f, string fill = DefaultFillColor, string stroke = DefaultStrokeColor, string viewBox = "0 0 20 20", string markerUnits = "strokeWidth", string orient = "auto", string strokeLineCap = null, float strokeWidth = 1f)
    {
        var ret = new XElement(XName.Get("marker", SvgNamespace),
            new XAttribute("id", id),
            new XAttribute("refX", refX),
            new XAttribute("refY", refY),
            new XAttribute("markerWidth", 20f * scale),
            new XAttribute("markerHeight", 20f * scale),
            new XAttribute("fill", fill),
            new XAttribute("stroke", stroke),
            new XAttribute("stroke-width", strokeWidth),
            element
            );

        if (markerUnits != null)
        {
            ret.Add(new XAttribute("markerUnits", markerUnits));
        }
        if (orient != null)
        {
            ret.Add(new XAttribute("orient", orient));
        }
        if (viewBox != null)
        {
            ret.Add(new XAttribute("viewBox", viewBox));
        }
        if (strokeLineCap != null)
        {
            ret.Add(new XAttribute("stroke-linecap", strokeLineCap));
        }
        return ret;
    }

    private static XElement CreatePolyLine(Collection<Point> waypoints, string fill = DefaultFillColor, string stroke = DefaultStrokeColor, float strokeWidth = 1f, string strokeLineJoin = null, string strokeLineCap = null, string strokeDashArray = null, string markerStart = null, string markerEnd = null)
    {
        var pointsBuilder = new StringBuilder();
        for (var i = 0; i < waypoints.Count; i++)
        {
            var x = waypoints[i].X;
            var y = waypoints[i].Y;
            if (i > 0)
            {
                pointsBuilder.Append(' ');
            }
            pointsBuilder.Append(CreateCI($"{x},{y}"));
        }
        var polyline = new XElement(XName.Get("polyline", SvgNamespace),
                    new XAttribute("points", pointsBuilder),
                    new XAttribute("fill", fill),
                    new XAttribute("stroke", stroke),
                    new XAttribute("stroke-width", strokeWidth));
        if (strokeLineJoin != null)
        {
            polyline.Add(new XAttribute("stroke-linejoin", strokeLineJoin));
        }
        if (strokeLineCap != null)
        {
            polyline.Add(new XAttribute("stroke-linecap", strokeLineCap));
        }
        if (markerEnd != null)
        {
            polyline.Add(new XAttribute("marker-end", $"url(#{markerEnd})"));
        }
        if (strokeDashArray != null)
        {
            polyline.Add(new XAttribute("stroke-dasharray", strokeDashArray));
        }
        if (markerStart != null)
        {
            polyline.Add(new XAttribute("marker-start", $"url(#{markerStart})"));
        }
        return polyline;
    }

    private static string CreatePathFromConnection(BpmnEdge edge, int cornerRadius = 0)
    {
        var path = CreateCI($"m {edge.WayPoints[0].X},{edge.WayPoints[0].Y} ");
        for (var i = 1; i < edge.WayPoints.Count; i++)
        {
            var pointBefore = edge.WayPoints[i - 1];
            var point = edge.WayPoints[i];
            var pointAfter = i < edge.WayPoints.Count - 1 ? edge.WayPoints[i + 1] : null;
            if(pointAfter is null || cornerRadius == 0)
            {
                path += CreateCI($"L {point.X},{point.Y} ");
                continue;
            }
            var effectiveRadius = Math.Min(Math.Min(cornerRadius, VectorLength(point.X - pointBefore.X, point.Y - pointBefore.Y)), VectorLength(pointAfter.X - point.X, pointAfter.Y - point.Y));
            if(effectiveRadius == 0)
            {
                path += CreateCI($"L {point.X},{point.Y} ");
                continue;
            }
            var beforePoint = GetPointAtLength(point, pointBefore, effectiveRadius);
            var beforePoint2 = GetPointAtLength(point, pointBefore, effectiveRadius * 0.5);
            var afterPoint = GetPointAtLength(point, pointAfter, effectiveRadius);
            var afterPoint2 = GetPointAtLength(point, pointAfter, effectiveRadius * 0.5);

            path += CreateCI($"L {beforePoint.X},{beforePoint.Y} ");
            path += CreateCI($"C {beforePoint2.X},{beforePoint2.Y},{afterPoint2.X},{afterPoint2.Y},{afterPoint.X},{afterPoint.Y}");
        }
        return path;
    }

    private static double VectorLength(double x, double y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

    private static Point GetPointAtLength(Point start, Point end, double length)
    {
        var deltaX = end.X - start.X;
        var deltaY = end.Y - start.Y;
        var totalLength = VectorLength(deltaX, deltaY);
        var percent = length / totalLength;
        return new Point
        {
            X = start.X + deltaX * percent,
            Y = start.Y + deltaY * percent,
        };
    }
    

    protected override void VisitBpmnEdge(BpmnEdge bpmnEdge)
    {
        var bpmnElement = _finder.FindBaseElement(_definitions, bpmnEdge.Element.Name);
        var fill = GetFillColor(bpmnEdge, "none");
        var stroke = GetStrokeColor(bpmnEdge);
        string markerStart = null;
        string markerEnd = null;
        string strokeLineJoin;
        string strokeLineCap = null;
        string strokeDashArray = null;
        int cornerRadius = 5;

        var group = new XElement(XName.Get("g", SvgNamespace), new XAttribute("data-element-id", bpmnElement.Id));
        string name = null;
        switch (bpmnElement)
        {
            case SequenceFlow sequenceFlow:
                name = sequenceFlow.Name;
                strokeLineJoin = "round";
                markerEnd = "sequenceflow-end";
                var source = _finder.FindBaseElement(_definitions, sequenceFlow.SourceRef);
                if (sequenceFlow.ConditionExpression != null && source is Activity)
                {
                    markerStart = "conditional-flow-marker";
                }
                if (source is IDefaultSequence defaultSequence && defaultSequence?.Default == sequenceFlow.Id)
                {
                    markerStart = "conditional-default-flow-marker";
                }
                group.Add(CreatePath(CreatePathFromConnection(bpmnEdge, cornerRadius), fill, stroke, strokeLineJoin: strokeLineJoin, strokeLineCap: strokeLineCap, strokeDashArray: strokeDashArray, markerStart: markerStart, markerEnd: markerEnd));
                if(_tokens.TryGetValue(sequenceFlow.Id, out var tokenCount) && tokenCount > 0)
                {
                    var p = GetTokenMarkerPoint(bpmnEdge.WayPoints, -30);
                    group.Add(CreateCircle((float)p.X, (float)p.Y, 10f, stroke: "none", fill: "red"));
                    AddLabel(group, $"{tokenCount}", $"{sequenceFlow.Id}_TokenCount", new Bounds { X = p.X - 10f, Y = p.Y - 10f, Height = 20f, Width = 20f }, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, "white", fontSize: 10f, fontStyle: FontStyle.Bold);
                }
                break;
            case Association association:
                strokeDashArray = "0.5, 5";
                strokeLineCap = "round";
                strokeLineJoin = "round";

                if (association.AssociationDirection != AssociationDirection.None)
                {
                    markerEnd = "association-end";
                }
                if (association.AssociationDirection == AssociationDirection.Both)
                {
                    markerStart = "association-start";
                }
                group.Add(CreatePolyLine(bpmnEdge.WayPoints, fill, stroke, strokeWidth: 2f, strokeLineJoin: strokeLineJoin, strokeLineCap: strokeLineCap, strokeDashArray: strokeDashArray, markerStart: markerStart, markerEnd: markerEnd));
                break;
            case DataAssociation:
                strokeDashArray = "0.5, 5";
                strokeLineCap = "round";
                strokeLineJoin = "round";
                markerEnd = "association-end";

                group.Add(CreatePolyLine(bpmnEdge.WayPoints, fill, stroke, strokeWidth: 2f, strokeLineJoin: strokeLineJoin, strokeLineCap: strokeLineCap, strokeDashArray: strokeDashArray, markerStart: markerStart, markerEnd: markerEnd));
                break;
            case MessageFlow:
                //TODO
                break;
        }
        if (bpmnEdge.Label != null && name != null)
        {
            AddLabel(group, name, bpmnElement.Id, bpmnEdge.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, stroke, 10f);
        }

        _currentGroup.Add(group);
    }

    private static Point GetTokenMarkerPoint(Collection<Point> wayPoints, float offset)
    {
        var last = wayPoints[^1];
        var beforeLast = wayPoints[^2];
        var w = last.X - beforeLast.X;
        var h = last.Y - beforeLast.Y;
        var z = Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2));
        var zo = z + offset;
        var nw = w * zo / z;
        var nh = h * zo / z;
        return new Point { X = beforeLast.X + nw, Y = beforeLast.Y + nh };
    }

    protected override void VisitBpmnShape(BpmnShape bpmnShape)
    {
        try
        {
            var bpmnElement = _finder.FindBaseElement(_definitions, bpmnShape.Element.Name);
            switch (bpmnElement)
            {
                case Abstractions.Model.Task task:
                    AddTaskShape(bpmnShape, task);
                    break;
                case Event @event:
                    AddEventShape(bpmnShape, @event);
                    break;
                case TextAnnotation textAnnotation:
                    AddTextAnnotation(bpmnShape, textAnnotation);
                    break;
                case Gateway gateway:
                    AddGatewayShape(bpmnShape, gateway);
                    break;
                case Activity activity:
                    AddActivityShape(bpmnShape, activity);
                    break;
            }
        }
        catch
        {
            throw;
        }
    }

    private void AddTextAnnotation(BpmnShape bpmnShape, TextAnnotation textAnnotation)
    {
        var group = new XElement(XName.Get("g", SvgNamespace), 
            new XAttribute("data-element-id", textAnnotation.Id),
            new XAttribute("transform", $"matrix(1 0 0 1 {bpmnShape.Bounds.X} {bpmnShape.Bounds.Y})"));
        group.Add(CreateRectangle((float)bpmnShape.Bounds.Width, (float) bpmnShape.Bounds.Height, 0, fill: "none", stroke: "none"));
        group.Add(CreatePath(
            Path.TextAnnotation.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0f, 0f),
            stroke: GetStrokeColor(bpmnShape)
            ));
        _currentGroup.Add(group);
        var text = textAnnotation.Text?.InnerText ?? string.Empty;
        AddLabel(group.Parent, text, textAnnotation.Id, bpmnShape.Bounds, HorizontalAlignment.Center, VerticalAlignment.Top, 5f, GetStrokeColor(bpmnShape));
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex SplitRegex();
    
    private static void AddLabel(XElement group, string text, string id, Bounds bounds, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float padding, string stroke, float fontSize = 12f, FontStyle fontStyle = FontStyle.Regular, bool addGroup = true)
    {
        var font = SystemFonts.CreateFont("Arial", fontSize, fontStyle);
        var renderOptions = new TextOptions(font);
        var size = TextMeasurer.MeasureSize(text, renderOptions);
        var paddedWidth = bounds.Width - 2 * padding;
        var paddedHeight = bounds.Height - 2 * padding;
        var lines = new List<string>();
        if (size.Width > paddedWidth || size.Height > paddedHeight)
        {
            var words = SplitRegex().Split(text);
            var builder = new StringBuilder();
            var width = 0f;

            foreach (var word in words)
            {
                var t = " " + word;
                size = TextMeasurer.MeasureSize(t, renderOptions);
                width += size.Width;
                if (width <= paddedWidth)
                {
                    builder.Append(t);
                }
                else
                {
                    lines.Add(builder.ToString().Trim());
                    builder.Clear();
                    builder.Append(word);
                    width = size.Width;
                }
            }
            if (builder.Length > 0)
            {
                lines.Add(builder.ToString().Trim());
            }
        }
        else
        {
            lines.Add(text);
        }
        var sizes = lines.Select(l => TextMeasurer.MeasureSize(l, renderOptions)).ToList();
        var totalHeight = sizes.Sum(x => x.Height);
        var totalWidth = sizes.Sum(x => x.Width);

        var y = verticalAlignment == VerticalAlignment.Center ? ((float)bounds.Height - totalHeight) / 2f : padding;
        y -= sizes[0].Height / 4;
        (var fontWeight, var style) = fontStyle switch
        {
            FontStyle.Bold => ("bold", "normal"),
            FontStyle.BoldItalic => ("bold", "italic"),
            FontStyle.Italic => ("normal", "italic"),
            _ => ("normal", "normal")
        };
        var textElement = new XElement(XName.Get("text", SvgNamespace),
            //    new XAttribute("x", bounds.X),
            //    new XAttribute("y", bounds.Y),
            new XAttribute("font-family", "Arial"),
            new XAttribute("font-size", fontSize),
            new XAttribute("font-weight", fontWeight),
            new XAttribute("font-style", style),
            new XAttribute("fill", stroke ?? DefaultStrokeColor));

        

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var lineSize = sizes[i];

            var x = 0f;
            y += lineSize.Height;

            x = horizontalAlignment switch
            {
                HorizontalAlignment.Left => padding,
                HorizontalAlignment.Right => (float)bounds.Width - padding - lineSize.Width,
                _ => Math.Max(((float)bounds.Width - lineSize.Width) / 2f, padding),
            };
            var tspan = new XElement(XName.Get("tspan", SvgNamespace),
                new XAttribute("x", x),
                new XAttribute("y", y),
                new XText(line)
                );
            textElement.Add(tspan);
        }

        if (addGroup)
        {
            var textGroup = new XElement(XName.Get("g", SvgNamespace),
                new XAttribute("transform", CreateCI($"matrix(1 0 0 1 {bounds.X} {bounds.Y})")),
                new XAttribute("data-element-id", $"{id}_Label"),
                textElement);
            group.Add(textGroup);
        }
        else
        {
            group.Add(textElement);
        }
    }

    private void AddActivityShape(BpmnShape bpmnShape, Activity activity, params object[] extraAttrs)
    {
        var group = new XElement(XName.Get("g", SvgNamespace), 
            new XAttribute("data-element-id", activity.Id),
            new XAttribute("transform", CreateCI($"matrix(1 0 0 1 {bpmnShape.Bounds.X} {bpmnShape.Bounds.Y})")));
        var fill = GetFillColor(bpmnShape);
        var stroke = GetStrokeColor(bpmnShape);
        float strokeWidth = 1f;
        string strokeDashArray = null;
        bool expanded;

        switch (activity)
        {
            case SubProcess subProcess:

                if (subProcess.TriggeredByEvent)
                {
                    strokeDashArray = "0, 5.5";
                    strokeWidth = 2.5f;
                }
                expanded = IsExpanded(bpmnShape, activity);
                AddEmbeddedLabel(group, bpmnShape, subProcess, HorizontalAlignment.Center, expanded ? VerticalAlignment.Top : VerticalAlignment.Center, addGroup: false);
                if (expanded)
                {
                    AttachTaskMarkers(group, bpmnShape, subProcess);
                }
                else
                {
                    AttachTaskMarkers(group, bpmnShape, subProcess, "SubProcessMarker");
                }
                break;
            case CallActivity callActivity:
                strokeWidth = 5f;
                expanded = IsExpanded(bpmnShape, activity);
                AddEmbeddedLabel(group, bpmnShape, callActivity, HorizontalAlignment.Center, expanded ? VerticalAlignment.Top : VerticalAlignment.Center);
                if (expanded)
                {
                    AttachTaskMarkers(group, bpmnShape, callActivity);
                }
                else
                {
                    AttachTaskMarkers(group, bpmnShape, callActivity, "SubProcessMarker");
                }
                break;
        }

        group.AddFirst(CreateRectangle((float) bpmnShape.Bounds.Width, (float) bpmnShape.Bounds.Height, TaskBorderRadius, fill: fill, stroke: stroke, strokeWidth: strokeWidth, strokeDashArray: strokeDashArray));
        _currentGroup.Add(group);

    }

    private bool IsExpanded(DiagramElement elem, BaseElement activity)
    {
        return (activity, elem) switch
        {
            (CallActivity, _) => false,
            (SubProcess, BpmnPlane) => true,
            (SubProcess, BpmnShape s) => s.IsExpanded,
            (Participant p, _) => p.ProcessRef is not null,
            _ => true
        };
    }

    private void AddGatewayShape(BpmnShape bpmnShape, Gateway gateway)
    {
        var b = bpmnShape.Bounds;
        var cx = (float) b.Width / 2f;
        var cy = (float) b.Height / 2f;
        var group = new XElement(XName.Get("g", SvgNamespace),
            new XAttribute("data-element-id", gateway.Id),
            new XAttribute("transform", CreateCI($"matrix(1 0 0 1 {b.X} {b.Y})")),
            new XElement(XName.Get("polygon", SvgNamespace),
                new XAttribute("points", "25,0 50,25 25,50 0,25"),
                new XAttribute("fill", "none"),
                new XAttribute("stroke", "black"),
                new XAttribute("stroke-width", 2)
            ));
        _currentGroup.Add(group);

        switch (gateway)
        {
            case ParallelGateway:
                group.Add(CreatePath(
                    Path.GatewayParallel.GetScaledPath(0.6f, (float)b.Width, (float)b.Height, 0.46f, 0.2f),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case ExclusiveGateway:
                group.Add(CreatePath(
                    Path.GatewayExclusive.GetScaledPath(0.4f, (float)b.Width, (float)b.Height, 0.32f, 0.3f),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case ComplexGateway:
                group.Add(CreatePath(
                    Path.GatewayComplex.GetScaledPath(0.5f, (float)b.Width, (float)b.Height, 0.46f, 0.26f),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case InclusiveGateway:
                group.Add(CreateCircle(cx, cy, (float)(b.Height * 0.24d), strokeWidth: 2.5f));
                break;
            case EventBasedGateway eventBasedGateway:

                group.Add(CreateCircle(cx, cy, (float)(b.Height * 0.20d), strokeWidth: 1f));
                switch (eventBasedGateway.EventGatewayType)
                {
                    case EventBasedGatewayType.Parallel:
                        group.Add(CreatePath(
                            Path.GatewayParallel.GetScaledPath(0.4f, (float)b.Width, (float)b.Height, 0.474f, 0.296f),
                            strokeWidth: 1f));
                        break;
                    case EventBasedGatewayType.Exclusive:
                        if (!eventBasedGateway.Instantiate)
                        {
                            group.Add(CreateCircle(cx, cy, (float)(b.Height * 0.26d), strokeWidth: 1f));
                        }
                        break;
                }
                group.Add(CreatePath(
                    Path.GatewayEventBased.GetScaledPath(0.18f, (float)b.Width, (float)b.Height, 0.36f, 0.44f)));
                break;
        }
        if (bpmnShape.Label != null)
        {
            AddLabel(group.Parent, gateway.Name, gateway.Id, bpmnShape.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, DefaultStrokeColor);
        }
        
    }

    private string GetStrokeColor(DiagramElement element, string defaultColor = DefaultStrokeColor)
    {
        var biocStroke = element.ExtensionAttributes.FirstOrDefault(x => x.LocalName == "stroke" && x.NamespaceURI == BiColorNamespace);
        return biocStroke?.Value ?? defaultColor ?? "black";
    }

    private string GetFillColor(DiagramElement element, string defaultColor = DefaultFillColor)
    {
        var biocFill = element.ExtensionAttributes.FirstOrDefault(x => x.LocalName == "fill" && x.NamespaceURI == BiColorNamespace);
        return biocFill?.Value ?? defaultColor ?? "none";
    }

    private XElement CreatePath(string d, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeLineCap = null, string strokeLineJoin = null, string strokeDashArray = null, string transform = null, string markerStart = null, string markerEnd = null)
    {
        var ret = new XElement(XName.Get("path", SvgNamespace),
            new XAttribute("d", d),
            new XAttribute("fill", fill),
            new XAttribute("stroke", stroke),
            new XAttribute("stroke-width", strokeWidth));

        if (strokeLineCap != null)
        {
            ret.Add(new XAttribute("stroke-linecap", strokeLineCap));
        }
        if (strokeLineJoin != null)
        {
            ret.Add(new XAttribute("stroke-linejoin", strokeLineJoin));
        }
        if (strokeDashArray != null)
        {
            ret.Add(new XAttribute("stroke-dasharray", strokeDashArray));
        }
        if (transform != null)
        {
            ret.Add(new XAttribute("transform", transform));
        }
        if (markerEnd != null)
        {
            ret.Add(new XAttribute("marker-end", $"url(#{markerEnd})"));
        }
        if (markerStart != null)
        {
            ret.Add(new XAttribute("marker-start", $"url(#{markerStart})"));
        }

        return ret;
    }

    private XElement CreateCircle(float cx, float cy, float r, float offset = 0f, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeDashArray = null, string strokeLineCap = null, float? fillOpacity = null)
    {
        var ret = new XElement(XName.Get("circle", SvgNamespace),
                    new XAttribute("cx", cx),
                    new XAttribute("cy", cy),
                    new XAttribute("r", r - offset),
                    new XAttribute("fill", fill),
                    new XAttribute("stroke", stroke),
                    new XAttribute("stroke-width", strokeWidth));
        if (strokeDashArray != null)
        {
            ret.Add(new XAttribute("stroke-dasharray", strokeDashArray));
        }
        if (strokeLineCap != null)
        {
            ret.Add(new XAttribute("stroke-linecap", strokeLineCap));
        }
        if (fillOpacity != null)
        {
            ret.Add(new XAttribute("fill-opacity", fillOpacity.Value));
        }
        return ret;
    }

    private XElement CreateRectangle(float width, float height, float borderRadius = 0f, float offset = 0f, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeDashArray = null, string strokeLineCap = null, float? fillOpacity = null)
    {
        var ret = new XElement(XName.Get("rect", SvgNamespace),
                    new XAttribute("x", offset),
                    new XAttribute("y", offset),
                    new XAttribute("width", width - offset * 2),
                    new XAttribute("height", height - offset * 2),
                    new XAttribute("rx", borderRadius),
                    new XAttribute("ry", borderRadius),
                    new XAttribute("fill", fill),
                    new XAttribute("stroke", stroke),
                    new XAttribute("stroke-width", strokeWidth));
        if (strokeDashArray != null)
        {
            ret.Add(new XAttribute("stroke-dasharray", strokeDashArray));
        }
        if (strokeLineCap != null)
        {
            ret.Add(new XAttribute("stroke-linecap", strokeLineCap));
        }
        if (fillOpacity != null)
        {
            ret.Add(new XAttribute("fill-opacity", fillOpacity.Value));
        }
        return ret;
    }

    private void AddEventShape(BpmnShape bpmnShape, Event @event)
    {
        var strokeWidth = 2f;
        string strokeDashArray = null;
        string strokeLineCap = null;
        string fill = GetFillColor(bpmnShape);
        string stroke = GetStrokeColor(bpmnShape);
        float? fillOpacity = null;

        var eventGroup = new XElement(XName.Get("g", SvgNamespace),
            new XAttribute("transform", CreateCI($"matrix(1 0 0 1 {bpmnShape.Bounds.X} {bpmnShape.Bounds.Y})")),
            new XAttribute("data-element-id", @event.Id));

        switch (@event)
        {
            case StartEvent startEvent:
                if (!startEvent.IsInterrupting)
                {
                    strokeDashArray = "6";
                    strokeLineCap = "round";
                }
                break;
            case EndEvent:
                strokeWidth *= 2;
                break;
            case IntermediateCatchEvent:
            case IntermediateThrowEvent:
                strokeWidth = 1f;
                eventGroup.Add(CreateCircle((float)bpmnShape.Bounds.Width / 2f, (float) bpmnShape.Bounds.Height / 2f, (float)bpmnShape.Bounds.Width / 2f, InnerOuterDistance, strokeWidth: 1f, fill: GetFillColor(bpmnShape, "none"), stroke: GetStrokeColor(bpmnShape)));
                break;
            case BoundaryEvent boundaryEvent:
                strokeWidth = 1f;
                if (!boundaryEvent.CancelActivity)
                {
                    strokeDashArray = "6";
                    strokeLineCap = "round";
                }
                fillOpacity = 1f;
                eventGroup.Add(CreateCircle((float)bpmnShape.Bounds.Width / 2f, (float)bpmnShape.Bounds.Height / 2f, (float)bpmnShape.Bounds.Width / 2f, InnerOuterDistance, strokeWidth: 1f, fill: "none", stroke: stroke, strokeDashArray: strokeDashArray, strokeLineCap: strokeLineCap));
                break;
        }
        eventGroup.AddFirst(CreateCircle((float)(bpmnShape.Bounds.Width / 2f), (float)(bpmnShape.Bounds.Height / 2f), (float)bpmnShape.Bounds.Width / 2f, fill: fill, stroke: stroke, strokeWidth: strokeWidth, strokeDashArray: strokeDashArray, strokeLineCap: strokeLineCap, fillOpacity: fillOpacity));
        _currentGroup.Add(eventGroup);
        AddEventContent(bpmnShape, @event, eventGroup);
    }

    private bool IsTypedEvent<T>(Event @event, Func<Event, bool> filter = null) where T : EventDefinition
    {
        static bool NoFilter(Event _) => true;
        Func<Event, bool> predicate = filter ?? NoFilter;

        if (@event is IEventDefinitions eventDefinitions && predicate(@event))
        {
            if (eventDefinitions.EventDefinitions.OfType<T>().Any())
            {
                return true;
            }
            var finder = new ElementFinder();
            return eventDefinitions.EventDefinitionRefs.Any(x => finder.FindBaseElement(_definitions, x.Name) is T);
        }
        return false;
    }

    private void AddEventContent(BpmnShape bpmnShape, Event @event, XElement parent)
    {
        var isThrowing = @event is ThrowEvent;
        if (IsTypedEvent<MessageEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventMessage.GetScaledPath(0.9f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.235f, 0.315f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : GetFillColor(bpmnShape),
                stroke: isThrowing ? GetFillColor(bpmnShape) : GetStrokeColor(bpmnShape),
                strokeWidth: 1f
                ));
        }
        if (IsTypedEvent<TimerEventDefinition>(@event))
        {
            parent.Add(CreateCircle((float)bpmnShape.Bounds.Width / 2f, (float)bpmnShape.Bounds.Height / 2f, (float)bpmnShape.Bounds.Width / 2f, (float)bpmnShape.Bounds.Height * 0.2f, stroke: GetStrokeColor(bpmnShape), fill: GetFillColor(bpmnShape)));
            parent.Add(CreatePath(
                Path.EventTimerWH.GetScaledPath(0.75f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.5f, 0.5f),
                strokeLineCap: "round",
                stroke: GetStrokeColor(bpmnShape)
                ));

            for (var i = 0; i < 12; i++)
            {
                var width = bpmnShape.Bounds.Width / 2;
                var height = bpmnShape.Bounds.Height / 2;

                parent.Add(CreatePath(
                    Path.EventTimerLine.GetScaledPath(0.75f, (float) bpmnShape.Bounds.Width, (float) bpmnShape.Bounds.Height, 0.5f, 0.5f),
                    strokeWidth: 1f,
                    strokeLineCap: "round",
                    transform: CreateCI($"rotate({i * 30},{height},{width})"),
                    stroke: GetStrokeColor(bpmnShape)
                    ));
            }
        }
        if (IsTypedEvent<ConditionalEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventConditional.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0f, 0f),
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<SignalEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventSignal.GetScaledPath(0.9f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.5f, 0.2f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event) && IsTypedEvent<TerminateEventDefinition>(@event, x => x is CatchEvent catchEvent && !catchEvent.ParallelMultiple))
        {
            parent.Add(CreatePath(
                Path.EventMultiple.GetScaledPath(1.1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.222f, 0.36f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event) && IsTypedEvent<TerminateEventDefinition>(@event, x => x is CatchEvent catchEvent && catchEvent.ParallelMultiple))
        {
            parent.Add(CreatePath(
                Path.EventParallelMultiple.GetScaledPath(1.2f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.458f, 0.194f),
                strokeWidth: 1f,
                fill: GetStrokeColor(bpmnShape),
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<EscalationEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventEscalation.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.5f, 0.2f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<LinkEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventLink.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.57f, 0.263f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<ErrorEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventError.GetScaledPath(1.1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.2f, 0.722f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventCancel45.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.638f, -0.055f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape),
                transform: "rotate(45)"
                ));
        }
        if (IsTypedEvent<CompensateEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventCompensation.GetScaledPath(1f, bpmnShape.Bounds.Width, bpmnShape.Bounds.Height, 0.22f, 0.5f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<TerminateEventDefinition>(@event))
        {
            parent.Add(CreateCircle((float) bpmnShape.Bounds.Width / 2, (float) bpmnShape.Bounds.Height / 2, 8f, strokeWidth: 4f, fill: GetStrokeColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)));
        }
        if (bpmnShape.Label != null)
        {
            AddLabel(parent.Parent, @event.Name, @event.Id, bpmnShape.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, DefaultStrokeColor);
        }
    }

    private void AddTaskShape(BpmnShape bpmnShape, Abstractions.Model.Task task)
    {
        var taskGroup = new XElement(XName.Get("g", SvgNamespace), 
            new XAttribute("transform", CreateCI($"matrix(1 0 0 1 {bpmnShape.Bounds.X} {bpmnShape.Bounds.Y})")), 
            new XAttribute("data-element-id", task.Id));
        var fill = GetFillColor(bpmnShape);
        var stroke = GetStrokeColor(bpmnShape);
        float strokeWidth = 2f;

        switch (task)
        {
            case UserTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser1.GetScaledPath(15f, 12f),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser2.GetScaledPath(15f, 12f),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser3.GetScaledPath(15f, 12f),
                    strokeWidth: 0.5f, fill: GetStrokeColor(bpmnShape), stroke: "none"
                    ));
                break;
            case ServiceTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeService.GetScaledPath(12f, 18f),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeServiceFill.GetScaledPath(17.2f, 18f),
                    strokeWidth: 0f,
                    fill: GetFillColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeService.GetScaledPath(17f, 22f),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case ManualTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeManual.GetScaledPath(17f, 15f),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case SendTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeSend.GetScaledPath(1f, 21f, 14f, 0.285f, 0.357f),
                    strokeWidth: 1f, fill: GetStrokeColor(bpmnShape), stroke: GetFillColor(bpmnShape)
                    ));
                break;
            case ReceiveTask receiveTask:
                if (receiveTask.Instantiate)
                {
                    taskGroup.Add(CreateCircle(14f, 14f, 20f * 0.22f, strokeWidth: 1f));
                    taskGroup.Add(CreatePath(
                        Path.TaskTypeInstantiatingSend.GetScaledPath(7.77f, 9.52f),
                        strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                        ));
                }
                else
                {
                    taskGroup.Add(CreatePath(
                        Path.TaskTypeSend.GetScaledPath(0.9f, 21f, 14f, 0.3f, 0.4f),
                        strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                        ));
                }
                break;
            case ScriptTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeScript.GetScaledPath(15f, 20f),
                    strokeWidth: 1f, stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case BusinessRuleTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeBusinessRuleHeader.GetScaledPath(8f, 8f),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape, "#aaaaaa"), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeBusinessRuleMain.GetScaledPath(8f, 8f),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
        }

        taskGroup.AddFirst(CreateRectangle((float)bpmnShape.Bounds.Width, (float) bpmnShape.Bounds.Height, TaskBorderRadius, fill: fill, stroke: stroke, strokeWidth: strokeWidth));
        AttachTaskMarkers(taskGroup, bpmnShape, task);
        _currentGroup.Add(taskGroup);
        AddEmbeddedLabel(_currentGroup, bpmnShape, task, HorizontalAlignment.Center, VerticalAlignment.Center);
    }

    private void AddEmbeddedLabel(XElement parent, BpmnShape bpmnShape, Activity activity, HorizontalAlignment hAlign = HorizontalAlignment.Center, VerticalAlignment vAlign = VerticalAlignment.Center, bool addGroup = true)
    {
        AddLabel(parent, activity.Name, activity.Id, bpmnShape.Bounds, hAlign, vAlign, 5f, GetStrokeColor(bpmnShape), addGroup: addGroup);
    }

    private void AttachTaskMarkers(XElement taskGroup, BpmnShape bpmnShape, Activity activity, params string[] markers)
    {
        var subProcess = markers.Contains("SubProcessMarker");
        var seq = subProcess ? -21 : -5;
        var parallel = subProcess ? -22 : -6;
        var compensation = subProcess ? -42 : -27;
        var loop = subProcess ? -18 : 0;
        var adhoc = 10;

        foreach(var marker in markers)
        {
            CreateTaskMarker(taskGroup, marker, bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
        }

        if(activity.IsForComposation)
        {
            CreateTaskMarker(taskGroup, "CompensationMarker", bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
        }

        if(activity is AdHocSubProcess)
        {
            CreateTaskMarker(taskGroup, "AdhocMarker", bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
        }

        if(activity.LoopCharacteristics is not null)
        {
            if(activity.LoopCharacteristics is MultiInstanceLoopCharacteristics milc)
            {
                if(milc.IsSequential)
                {
                    CreateTaskMarker(taskGroup, "SequentialMarker", bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
                }
                else
                {
                    CreateTaskMarker(taskGroup, "ParallelMarker", bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
                }
            }
            else
            {
                CreateTaskMarker(taskGroup, "LoopMarker", bpmnShape, activity, parallel, seq, compensation, loop, adhoc);
            }
        }
    }

    private void CreateTaskMarker(XElement taskGroup, string marker, BpmnShape bpmnShape, Activity activity, float parallel = 0f, float sequential = 0f, float compensation = 0f, float loop = 0f, float adhoc = 0f)
    {
        var width = bpmnShape.Bounds.Width;
        var height = bpmnShape.Bounds.Height;
        switch (marker)
        {
            case "ParticipantMultiplicityMarker":
                taskGroup.Add(CreatePath(Path.MarkerParallel.GetScaledPath(1f, width, height, (float) ((width / 2 - 6) / width), (float) ((height - 15) / height))));
                break;
            case "SubProcessMarker":
                var markerRect = CreateRectangle(14, 14, 0, strokeWidth: 1f);
                markerRect.SetAttributeValue("transform", CreateCI($"translate({width / 2 - 7.5} {height - 20})"));
                taskGroup.Add(markerRect);
                taskGroup.Add(CreatePath(Path.MarkerSubProcess.GetScaledPath(1.5f, width, height, (float)((width / 2 - 7.5) / width), (float)((height - 20) / height))));
                break;
            case "ParallelMarker":
                taskGroup.Add(CreatePath(Path.MarkerParallel.GetScaledPath(1f, width, height, (float)((width / 2 + parallel) / width), (float)((height - 20) / height))));
                break;
            case "SequentialMarker":
                taskGroup.Add(CreatePath(Path.MarkerSequential.GetScaledPath(1f, width, height, (float)((width / 2 + sequential) / width), (float)((height - 19) / height))));
                break;
            case "CompensationMarker":
                taskGroup.Add(CreatePath(Path.MarkerCompensation.GetScaledPath(1f, width, height, (float)((width / 2 + compensation) / width), (float)((height - 13) / height)), strokeWidth: 1f));
                break;
            case "LoopMarker":
                taskGroup.Add(CreatePath(Path.MarkerLoop.GetScaledPath(1f, width, height, (float)((width / 2 + loop) / width), (float)((height - 7) / height)), strokeWidth: 1.5f, fill: "none"));
                break;
            case "AdhocMarker":
                taskGroup.Add(CreatePath(Path.MarkerAdHoc.GetScaledPath(1f, width, height, (float)((width / 2 + adhoc) / width), (float)((height - 15) / height)), strokeWidth: 1f));
                break;
        }
    }

    private static (int width, int height) CalculateWidthAndHeight(Plane plane)
    {
        var maxX = new int[3];
        var maxY = new int[3];
        maxX[0] = GetMax(plane.Elements.OfType<Shape>(), s => s.Bounds.X + s.Bounds.Width);
        maxY[0] = GetMax(plane.Elements.OfType<Shape>(), s => s.Bounds.Y + s.Bounds.Height);
        maxX[1] = GetMax(plane.Elements.OfType<Label>(), s => s.Bounds.X + s.Bounds.Width);
        maxY[1] = GetMax(plane.Elements.OfType<Label>(), s => s.Bounds.Y + s.Bounds.Height);
        maxX[2] = GetMax(plane.Elements.OfType<Edge>().SelectMany(x => x.WayPoints), s => s.X);
        maxY[2] = GetMax(plane.Elements.OfType<Edge>().SelectMany(x => x.WayPoints), s => s.Y);

        var minX = new int[3];
        var minY = new int[3];
        minX[0] = GetMin(plane.Elements.OfType<Shape>(), s => s.Bounds.X + s.Bounds.Width);
        minY[0] = GetMin(plane.Elements.OfType<Shape>(), s => s.Bounds.Y + s.Bounds.Height);
        minX[1] = GetMin(plane.Elements.OfType<Label>(), s => s.Bounds.X + s.Bounds.Width);
        minY[1] = GetMin(plane.Elements.OfType<Label>(), s => s.Bounds.Y + s.Bounds.Height);
        minX[2] = GetMin(plane.Elements.OfType<Edge>().SelectMany(x => x.WayPoints), s => s.X);
        minY[2] = GetMin(plane.Elements.OfType<Edge>().SelectMany(x => x.WayPoints), s => s.Y);

        var xmin = minX.Min();
        var ymin = minY.Min();
        var xmax = maxX.Max();
        var ymax = maxY.Max();

        return (xmax + xmin, ymax + ymin);
    }

    private static int GetMax<T>(IEnumerable<T> elements, Func<T, double> accessor)
    {
        if (elements.Any())
        {
            return (int)Math.Ceiling(elements.Max(accessor));
        }
        return int.MinValue;
    }

    private static int GetMin<T>(IEnumerable<T> elements, Func<T, double> accessor)
    {
        if (elements.Any())
        {
            return (int)Math.Ceiling(elements.Min(accessor));
        }
        return int.MaxValue;
    }
}
