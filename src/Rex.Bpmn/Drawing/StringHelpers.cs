using System.Globalization;
using System.Runtime.CompilerServices;

namespace Rex.Bpmn.Drawing;

internal static class StringHelpers
{

    public static string CreateCI(FormattableString fs)
    {
        return fs.ToString(CultureInfo.InvariantCulture);
    }
}
