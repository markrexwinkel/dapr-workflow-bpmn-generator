using Rex.Bpmn.Abstractions.Model;

namespace Rex.Bpmn.Drawing;

public static class BoundsExtensions
{
    public static Point Offset(this Bounds bounds, float cx, float cy)
    {
        return new Point
        {
            X = bounds.X + cx,
            Y = bounds.Y + cy
        };
    }
}
