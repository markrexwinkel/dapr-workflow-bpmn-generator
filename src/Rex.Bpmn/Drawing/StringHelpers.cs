using System.Globalization;
using System.Runtime.CompilerServices;

namespace Rex.Bpmn.Drawing;

internal static class StringHelpers
{

    public static string CreateCI(ref DefaultInterpolatedStringHandler handler)
    {
        return string.Create(CultureInfo.InvariantCulture, ref handler);
    }
}
