using Rex.Bpmn.Abstractions;
using Rex.Bpmn.Abstractions.Model;
using SixLabors.Fonts;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Rex.Bpmn.Drawing;

public class DiagramToSvgVisitor(Definitions definitions) : BpmnModelVisitor
{
    private const string SvgNamespace = "http://www.w3.org/2000/svg";
    private const string XLinkNamespace = "http://www.w3.org/1999/xlink";
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

    public XDocument CreateSvgDiagram(BpmnDiagram diagram)
    {
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
        AddSymbols();
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

    private XElement CreateMarker(string id, XElement element, float refX, float refY, float scale = 1f, string fill = DefaultFillColor, string stroke = DefaultStrokeColor, string viewBox = "0 0 20 20", string markerUnits = "strokeWidth", string orient = "auto", string strokeLineCap = null, float strokeWidth = 1f)
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

    private void AddSymbols()
    {
        _defs.Add(
            new XElement(XName.Get("symbol", SvgNamespace),
                new XAttribute("id", "userTaskSymbol"),
                //new XAttribute("viewBox", "0 0 20 20"),
                new XElement(XName.Get("path", SvgNamespace),
                        new XAttribute("fill", "none"),
                        new XAttribute("stroke", "black"),
                        new XAttribute("stroke-width", 1),
                        new XAttribute("d", $"c 0.909,-0.845 1.594,-2.049 1.594,-3.385 0,-2.554 -1.805,-4.62199999 -4.357,-4.62199999 -2.55199998,0 -4.28799998,2.06799999 -4.28799998,4.62199999 0,1.348 0.974,2.562 1.89599998,3.405 -0.52899998,0.187 -5.669,2.097 -5.794,4.7560005 v 6.718 h 17 v -6.718 c 0,-2.2980005 -5.5279996,-4.5950005 -6.0509996,-4.7760005 z m -8,6 l 0,5.5 m 11,0 l 0,-5")),

                new XElement(XName.Get("path", SvgNamespace),
                    new XAttribute("fill", "none"),
                    new XAttribute("stroke", "black"),
                    new XAttribute("stroke-width", 1),
                    new XAttribute("d", $"m 2.162,1.009 c 0,2.4470005 -2.158,4.4310005 -4.821,4.4310005 -2.66499998,0 -4.822,-1.981 -4.822,-4.4310005")),
                new XElement(XName.Get("path", SvgNamespace),
                    new XAttribute("d", $"-6.9,-3.80 c 0,0 2.25099998,-2.358 4.27399998,-1.177 2.024,1.181 4.221,1.537 4.124,0.965 -0.098,-0.57 -0.117,-3.79099999 -4.191,-4.13599999 -3.57499998,0.001 -4.20799998,3.36699999 -4.20699998,4.34799999 z")))
                    );
    }

    private XElement CreatePolyLine(Collection<Abstractions.Model.Point> waypoints, string fill = DefaultFillColor, string stroke = DefaultStrokeColor, float strokeWidth = 1f, string strokeLineJoin = null, string strokeLineCap = null, string strokeDashArray = null, string markerStart = null, string markerEnd = null)
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
            pointsBuilder.Append($"{x},{y}");
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

    private string CreatePathFromConnection(BpmnEdge edge)
    {
        var path = ((FormattableString)$"m {edge.WayPoints[0].X},{edge.WayPoints[0].Y} ").ToString(CultureInfo.InvariantCulture);
        for (var i = 1; i < edge.WayPoints.Count; i++)
        {
            path += ((FormattableString)$"L {edge.WayPoints[i].X},{edge.WayPoints[i].Y} ").ToString(CultureInfo.InvariantCulture);
        }
        return path;
    }

    protected override void VisitBpmnEdge(BpmnEdge bpmnEdge)
    {
        var bpmnElement = _finder.FindBaseElement(_definitions, bpmnEdge.Element.Name);
        var fill = GetFillColor(bpmnEdge);
        var stroke = GetStrokeColor(bpmnEdge);
        string markerStart = null;
        string markerEnd = null;
        string strokeLineJoin = null;
        string strokeLineCap = null;
        string strokeDashArray = null;

        var group = new XElement(XName.Get("g", SvgNamespace));
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
                group.Add(CreatePath(CreatePathFromConnection(bpmnEdge), fill, stroke, strokeLineJoin: strokeLineJoin, strokeLineCap: strokeLineCap, strokeDashArray: strokeDashArray, markerStart: markerStart, markerEnd: markerEnd));
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
            case DataAssociation dataInputAssociation:
                strokeDashArray = "0.5, 5";
                strokeLineCap = "round";
                strokeLineJoin = "round";
                markerEnd = "association-end";

                group.Add(CreatePolyLine(bpmnEdge.WayPoints, fill, stroke, strokeWidth: 2f, strokeLineJoin: strokeLineJoin, strokeLineCap: strokeLineCap, strokeDashArray: strokeDashArray, markerStart: markerStart, markerEnd: markerEnd));
                break;
            case MessageFlow messageFlow:
                //TODO
                break;
        }
        if (bpmnEdge.Label != null && name != null)
        {
            AddLabel(group, name, bpmnEdge.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, stroke, 10f);
        }

        _currentGroup.Add(group);
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
                    //case Gateway gateway:
                    //default:
                    //    throw new NotSupportedException("Element is not supported");
            }
            //base.VisitBpmnShape(bpmnShape);
        }
        catch
        {
            throw;
        }
    }

    private void AddTextAnnotation(BpmnShape bpmnShape, TextAnnotation textAnnotation)
    {
        var group = new XElement(XName.Get("g", SvgNamespace));
        group.Add(CreateRectangle(bpmnShape.Bounds, 0, "none", "none"));
        group.Add(CreatePath(
            Path.TextAnnotation.GetScaledPath(1f, bpmnShape.Bounds, 0f, 0f),
            stroke: GetStrokeColor(bpmnShape)
            ));
        var text = textAnnotation.Text?.InnerText ?? string.Empty;
        AddLabel(group, text, bpmnShape.Bounds, HorizontalAlignment.Center, VerticalAlignment.Top, 5f, GetStrokeColor(bpmnShape));
        _currentGroup.Add(group);
    }

    private void AddSimpleText(XElement group, string text, Bounds bounds, string stroke)
    {
        var textElement = new XElement(XName.Get("text", SvgNamespace),
            new XAttribute("x", bounds.X),
            new XAttribute("y", bounds.Y),
            new XAttribute("font-family", "Arial"),
            new XAttribute("font-size", 12f),
            new XAttribute("stroke", stroke ?? DefaultStrokeColor),
            new XText(text));
        group.Add(textElement);
    }

    private void AddLabel(XElement group, string text, Bounds bounds, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float padding, string stroke, float fontSize = 12f)
    {
        var font = SystemFonts.CreateFont("Arial", fontSize);
        var renderOptions = new TextOptions(font);
        var size = TextMeasurer.MeasureSize(text, renderOptions);
        var paddedWidth = bounds.Width - 2 * padding;
        var paddedHeight = bounds.Height - 2 * padding;
        var lines = new List<string>();
        if (size.Width > paddedWidth || size.Height > paddedHeight)
        {
            var words = Regex.Split(text, @"\s+");
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
                    width = 0f;
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

        var textElement = new XElement(XName.Get("text", SvgNamespace),
            //    new XAttribute("x", bounds.X),
            //    new XAttribute("y", bounds.Y),
            new XAttribute("font-family", "Arial"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", stroke ?? DefaultStrokeColor));

        var textGroup = new XElement(XName.Get("g", SvgNamespace),
            new XAttribute("transform", $"matrix(1 0 0 1 {bounds.X} {bounds.Y})"),
            textElement);

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var lineSize = sizes[i];

            var x = 0f;
            y += lineSize.Height;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    x = padding;
                    break;
                case HorizontalAlignment.Right:
                    x = (float)bounds.Width - padding - lineSize.Width;
                    break;
                default:
                    x = Math.Max(((float)bounds.Width - lineSize.Width) / 2f, padding);
                    break;
            }

            var tspan = new XElement(XName.Get("tspan", SvgNamespace),
                new XAttribute("x", x),
                new XAttribute("y", y),
                new XText(line)
                );
            textElement.Add(tspan);
        }


        group.Add(textGroup);

    }

    private void AddActivityShape(BpmnShape bpmnShape, Activity activity)
    {
        var group = new XElement(XName.Get("g", SvgNamespace));
        var fill = GetFillColor(bpmnShape);
        var stroke = GetStrokeColor(bpmnShape);
        float strokeWidth = 1f;
        string strokeDashArray = null;

        switch (activity)
        {
            case SubProcess subProcess:

                if (subProcess.TriggeredByEvent)
                {
                    strokeDashArray = "1,2";
                }
                AddEmbeddedLabel(group, bpmnShape, subProcess, "center-top");
                AttachTaskMarkers(group, bpmnShape, subProcess);
                break;
            case CallActivity callActivity:
                strokeWidth = 5f;
                AttachTaskMarkers(group, bpmnShape, callActivity);
                break;
        }

        group.AddFirst(CreateRectangle(bpmnShape.Bounds, TaskBorderRadius, fill, stroke, strokeWidth, strokeDashArray: strokeDashArray));
        _currentGroup.Add(group);

    }

    private bool IsExpanded(BpmnShape bpmnShape, Activity activity)
    {
        return true;
    }

    private void AddGatewayShape(BpmnShape bpmnShape, Gateway gateway)
    {
        var b = bpmnShape.Bounds;
        var mh = b.Height / 2;
        var mw = b.Width / 2;
        var group = new XElement(XName.Get("g", SvgNamespace),
            new XElement(XName.Get("polygon", SvgNamespace),
                new XAttribute("points", ((FormattableString)$"{b.X},{b.Y + mh} {b.X + mw},{b.Y} {b.X + b.Width},{b.Y + mh} {b.X + mh},{b.Y + b.Height}").ToString(CultureInfo.InvariantCulture)),
                new XAttribute("fill", "none"),
                new XAttribute("stroke", "black"),
                new XAttribute("stroke-width", 2)
            ));

        switch (gateway)
        {
            case ParallelGateway parallelGateway:
                group.Add(CreatePath(
                    Path.GatewayParallel.GetScaledPath(new Path.ScaleParams { XScaleFactor = 0.6f, YScaleFactor = 0.6f, ContainerWidth = (float)bpmnShape.Bounds.Width, ContainerHeight = (float)bpmnShape.Bounds.Height, AbsolutePosition = new Abstractions.Model.Point { X = bpmnShape.Bounds.X, Y = bpmnShape.Bounds.Y }, Position = new Abstractions.Model.Point { X = 0.46, Y = 0.2 } }),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case ExclusiveGateway exclusiveGateway:
                group.Add(CreatePath(
                    Path.GatewayExclusive.GetScaledPath(new Path.ScaleParams { XScaleFactor = 0.4f, YScaleFactor = 0.4f, ContainerWidth = (float)bpmnShape.Bounds.Width, ContainerHeight = (float)bpmnShape.Bounds.Height, AbsolutePosition = new Abstractions.Model.Point { X = bpmnShape.Bounds.X, Y = bpmnShape.Bounds.Y }, Position = new Abstractions.Model.Point { X = 0.32, Y = 0.3 } }),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case ComplexGateway complexGateway:
                group.Add(CreatePath(
                    Path.GatewayComplex.GetScaledPath(new Path.ScaleParams { XScaleFactor = 0.5f, YScaleFactor = 0.5f, ContainerWidth = (float)bpmnShape.Bounds.Width, ContainerHeight = (float)bpmnShape.Bounds.Height, AbsolutePosition = new Abstractions.Model.Point { X = bpmnShape.Bounds.X, Y = bpmnShape.Bounds.Y }, Position = new Abstractions.Model.Point { X = 0.46, Y = 0.26 } }),
                    fill: "black",
                    strokeWidth: 1f));
                break;
            case InclusiveGateway inclusiveGateway:
                group.Add(CreateCircle(bpmnShape.Bounds, (float)(bpmnShape.Bounds.Height * 0.24d), strokeWidth: 2.5f));
                break;
            case EventBasedGateway eventBasedGateway:

                group.Add(CreateCircle(bpmnShape.Bounds, (float)(bpmnShape.Bounds.Height * 0.20d), strokeWidth: 1f));
                switch (eventBasedGateway.EventGatewayType)
                {
                    case EventBasedGatewayType.Parallel:
                        group.Add(CreatePath(
                            Path.GatewayParallel.GetScaledPath(new Path.ScaleParams { XScaleFactor = 0.4f, YScaleFactor = 0.4f, ContainerWidth = (float)bpmnShape.Bounds.Width, ContainerHeight = (float)bpmnShape.Bounds.Height, AbsolutePosition = new Abstractions.Model.Point { X = bpmnShape.Bounds.X, Y = bpmnShape.Bounds.Y }, Position = new Abstractions.Model.Point { X = 0.474, Y = 0.296 } }),
                            strokeWidth: 1f));
                        break;
                    case EventBasedGatewayType.Exclusive:
                        if (!eventBasedGateway.Instantiate)
                        {
                            group.Add(CreateCircle(bpmnShape.Bounds, (float)(bpmnShape.Bounds.Height * 0.26d), strokeWidth: 1f));
                        }
                        break;
                }
                group.Add(CreatePath(
                    Path.GatewayEventBased.GetScaledPath(new Path.ScaleParams { XScaleFactor = 0.18f, YScaleFactor = 0.18f, ContainerWidth = (float)bpmnShape.Bounds.Width, ContainerHeight = (float)bpmnShape.Bounds.Height, AbsolutePosition = new Abstractions.Model.Point { X = bpmnShape.Bounds.X, Y = bpmnShape.Bounds.Y }, Position = new Abstractions.Model.Point { X = 0.36, Y = 0.44 } })));
                break;
        }
        if (bpmnShape.Label != null)
        {
            AddLabel(group, gateway.Name, bpmnShape.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, DefaultStrokeColor);
        }
        _currentGroup.Add(group);
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

    private XElement CreateCircle(float r, Bounds bounds, float offset = 0f, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeDashArray = null, string strokeLineCap = null, float? fillOpacity = null)
    {
        var cx = (float)(bounds.X + bounds.Width / 2d);
        var cy = (float)(bounds.Y + bounds.Height / 2d);
        return CreateCircle(cx, cy, r, offset, fill, stroke, strokeWidth, strokeDashArray, strokeLineCap, fillOpacity);
    }

    private XElement CreateCircle(Bounds bounds, float offset = 0f, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeDashArray = null, string strokeLineCap = null, float? fillOpacity = null)
    {
        var r = (float)(bounds.Width / 2d);
        return CreateCircle(r, bounds, offset, fill, stroke, strokeWidth, strokeDashArray, strokeLineCap, fillOpacity);
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

    private XElement CreateRectangle(Bounds bounds, float borderRadius = 0f, string fill = "white", string stroke = "black", float strokeWidth = 2f, string strokeDashArray = null, string strokeLineCap = null, float? fillOpacity = null)
    {
        var ret = new XElement(XName.Get("rect", SvgNamespace),
                    new XAttribute("x", bounds.X),
                    new XAttribute("y", bounds.Y),
                    new XAttribute("width", bounds.Width),
                    new XAttribute("height", bounds.Height),
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
        var x = (float)(bpmnShape.Bounds.X + bpmnShape.Bounds.Width / 2d);
        var y = (float)(bpmnShape.Bounds.Y + bpmnShape.Bounds.Height / 2d);
        var r = (float)(bpmnShape.Bounds.Width / 2d);
        var strokeWidth = 2f;
        string strokeDashArray = null;
        string strokeLineCap = null;
        string fill = GetFillColor(bpmnShape);
        string stroke = GetStrokeColor(bpmnShape);
        float? fillOpacity = null;

        var eventGroup = new XElement(XName.Get("g", SvgNamespace));

        switch (@event)
        {
            case StartEvent startEvent:
                if (!startEvent.IsInterrupting)
                {
                    strokeDashArray = "6";
                    strokeLineCap = "round";
                }
                break;
            case EndEvent endEvent:
                strokeWidth *= 2;
                break;
            case IntermediateCatchEvent intermediateCatchEvent:
            case IntermediateThrowEvent intermediateThrowEvent:
                strokeWidth = 1f;
                eventGroup.Add(CreateCircle(bpmnShape.Bounds, InnerOuterDistance, strokeWidth: 1f, fill: GetFillColor(bpmnShape, "none"), stroke: GetStrokeColor(bpmnShape)));
                break;
            case BoundaryEvent boundaryEvent:
                strokeWidth = 1f;
                if (!boundaryEvent.CancelActivity)
                {
                    strokeDashArray = "6";
                    strokeLineCap = "round";
                }
                fillOpacity = 1f;
                eventGroup.Add(CreateCircle(bpmnShape.Bounds, InnerOuterDistance, strokeWidth: 1f, fill: "none", stroke: stroke, strokeDashArray: strokeDashArray, strokeLineCap: strokeLineCap));
                break;
        }
        eventGroup.AddFirst(CreateCircle(bpmnShape.Bounds, fill: fill, stroke: stroke, strokeWidth: strokeWidth, strokeDashArray: strokeDashArray, strokeLineCap: strokeLineCap, fillOpacity: fillOpacity));
        _currentGroup.Add(eventGroup);
        AddEventContent(bpmnShape, @event, eventGroup);
    }

    private bool IsTypedEvent<T>(Event @event, Func<Event, bool> filter = null) where T : EventDefinition
    {
        bool NoFilter(Event _) => true;
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
                Path.EventMessage.GetScaledPath(0.9f, bpmnShape.Bounds, 0.235f, 0.315f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : GetFillColor(bpmnShape),
                stroke: isThrowing ? GetFillColor(bpmnShape) : GetStrokeColor(bpmnShape),
                strokeWidth: 1f
                ));
        }
        if (IsTypedEvent<TimerEventDefinition>(@event))
        {
            parent.Add(CreateCircle(bpmnShape.Bounds, (float)bpmnShape.Bounds.Height * 0.2f, stroke: GetStrokeColor(bpmnShape), fill: GetFillColor(bpmnShape)));
            parent.Add(CreatePath(
                Path.EventTimerWH.GetScaledPath(0.75f, bpmnShape.Bounds, 0.5f, 0.5f),
                strokeLineCap: "square",
                stroke: GetStrokeColor(bpmnShape)
                ));

            for (var i = 0; i < 12; i++)
            {
                var width = bpmnShape.Bounds.Width / 2;
                var height = bpmnShape.Bounds.Height / 2;

                parent.Add(CreatePath(
                    Path.EventTimerLine.GetScaledPath(0.75f, bpmnShape.Bounds, 0.5f, 0.5f),
                    strokeWidth: 1f,
                    strokeLineCap: "square",
                    transform: ((FormattableString)$"rotate({i * 30},{height},{width})").ToString(CultureInfo.InvariantCulture),
                    stroke: GetStrokeColor(bpmnShape)
                    ));
            }
        }
        if (IsTypedEvent<ConditionalEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventConditional.GetScaledPath(1f, bpmnShape.Bounds, 0f, 0f),
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<SignalEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventSignal.GetScaledPath(0.9f, bpmnShape.Bounds, 0.5f, 0.2f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event) && IsTypedEvent<TerminateEventDefinition>(@event, x => x is CatchEvent catchEvent && !catchEvent.ParallelMultiple))
        {
            parent.Add(CreatePath(
                Path.EventMultiple.GetScaledPath(1.1f, bpmnShape.Bounds, 0.222f, 0.36f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event) && IsTypedEvent<TerminateEventDefinition>(@event, x => x is CatchEvent catchEvent && catchEvent.ParallelMultiple))
        {
            parent.Add(CreatePath(
                Path.EventParallelMultiple.GetScaledPath(1.2f, bpmnShape.Bounds, 0.458f, 0.194f),
                strokeWidth: 1f,
                fill: GetStrokeColor(bpmnShape),
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<EscalationEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventEscalation.GetScaledPath(1f, bpmnShape.Bounds, 0.5f, 0.2f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<LinkEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventLink.GetScaledPath(1f, bpmnShape.Bounds, 0.57f, 0.263f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<ErrorEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventError.GetScaledPath(1.1f, bpmnShape.Bounds, 0.2f, 0.722f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<CancelEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventCancel45.GetScaledPath(1f, bpmnShape.Bounds, 0.638f, -0.055f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape),
                transform: "rotate(45)"
                ));
        }
        if (IsTypedEvent<CompensateEventDefinition>(@event))
        {
            parent.Add(CreatePath(
                Path.EventCompensation.GetScaledPath(1f, bpmnShape.Bounds, 0.22f, 0.5f),
                fill: isThrowing ? GetStrokeColor(bpmnShape) : "none",
                strokeWidth: 1f,
                stroke: GetStrokeColor(bpmnShape)
                ));
        }
        if (IsTypedEvent<TerminateEventDefinition>(@event))
        {
            parent.Add(CreateCircle(bpmnShape.Bounds, 8f, strokeWidth: 4f, fill: GetStrokeColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)));
        }
        if (bpmnShape.Label != null)
        {
            AddLabel(parent, @event.Name, bpmnShape.Label.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 0f, DefaultStrokeColor);
        }
    }

    private void AddTaskShape(BpmnShape bpmnShape, Abstractions.Model.Task task)
    {
        var taskGroup = new XElement(XName.Get("g", SvgNamespace));
        var fill = GetFillColor(bpmnShape);
        var stroke = GetStrokeColor(bpmnShape);
        float strokeWidth = 2f;

        switch (task)
        {
            case UserTask userTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser1.GetScaledPath(bpmnShape.Bounds.Offset(15f, 12f)),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser2.GetScaledPath(bpmnShape.Bounds.Offset(15f, 12f)),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeUser3.GetScaledPath(bpmnShape.Bounds.Offset(15f, 12f)),
                    strokeWidth: 0.5f, fill: GetStrokeColor(bpmnShape), stroke: "none"
                    ));
                break;
            case ServiceTask serviceTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeService.GetScaledPath(bpmnShape.Bounds.Offset(12f, 18f)),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeServiceFill.GetScaledPath(bpmnShape.Bounds.Offset(17.2f, 18f)),
                    strokeWidth: 0f,
                    fill: GetFillColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeService.GetScaledPath(bpmnShape.Bounds.Offset(17f, 22f)),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case ManualTask manualTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeManual.GetScaledPath(bpmnShape.Bounds.Offset(17f, 15f)),
                    strokeWidth: 0.5f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case SendTask sendTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeSend.GetScaledPath(1f, (float)bpmnShape.Bounds.X, (float)bpmnShape.Bounds.Y, 21f, 14f, 0.285f, 0.357f),
                    strokeWidth: 1f, fill: GetStrokeColor(bpmnShape), stroke: GetFillColor(bpmnShape)
                    ));
                break;
            case ReceiveTask receiveTask:
                if (receiveTask.Instantiate)
                {
                    taskGroup.Add(CreateCircle((float)bpmnShape.Bounds.X + 14f, (float)bpmnShape.Bounds.Y + 14f, 20f * 0.22f, strokeWidth: 1f));
                    taskGroup.Add(CreatePath(
                        Path.TaskTypeInstantiatingSend.GetScaledPath(7.77f, 9.52f),
                        strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                        ));
                }
                else
                {
                    taskGroup.Add(CreatePath(
                        Path.TaskTypeSend.GetScaledPath(0.9f, (float)bpmnShape.Bounds.X, (float)bpmnShape.Bounds.Y, 21f, 14f, 0.3f, 0.4f),
                        strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                        ));
                }
                break;
            case ScriptTask scriptTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeScript.GetScaledPath(bpmnShape.Bounds.Offset(15f, 20f)),
                    strokeWidth: 1f, stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
            case BusinessRuleTask businessRuleTask:
                taskGroup.Add(CreatePath(
                    Path.TaskTypeBusinessRuleHeader.GetScaledPath(bpmnShape.Bounds.Offset(8f, 8f)),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape, "#aaaaaa"), stroke: GetStrokeColor(bpmnShape)
                    ));
                taskGroup.Add(CreatePath(
                    Path.TaskTypeBusinessRuleMain.GetScaledPath(bpmnShape.Bounds.Offset(8f, 8f)),
                    strokeWidth: 1f, fill: GetFillColor(bpmnShape), stroke: GetStrokeColor(bpmnShape)
                    ));
                break;
        }

        taskGroup.AddFirst(CreateRectangle(bpmnShape.Bounds, TaskBorderRadius, fill, stroke, strokeWidth));
        AddEmbeddedLabel(taskGroup, bpmnShape, task, "center-middle");
        AttachTaskMarkers(taskGroup, bpmnShape, task);
        _currentGroup.Add(taskGroup);
    }

    private void AddEmbeddedLabel(XElement parent, BpmnShape bpmnShape, Activity activity, string align)
    {
        AddLabel(parent, activity.Name, bpmnShape.Bounds, HorizontalAlignment.Center, VerticalAlignment.Center, 5f, GetStrokeColor(bpmnShape));
    }



    private void AttachTaskMarkers(XElement taskGroup, BpmnShape bpmnShape, Activity activity, params string[] markers)
    {
        //TODO
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
