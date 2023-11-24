using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace Rex.Bpmn.Dapr.Workflow.Generator;

internal static class StringExtensions
{
    public static string Capatilize(this string s)
    {
        if(string.IsNullOrEmpty(s))
        {
            return s;
        }
        return char.ToUpperInvariant(s[0]) + (s.Length > 1 ? s.Substring(1) : string.Empty);
    }

    public static string Uncapatilize(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return s;
        }
        return char.ToLowerInvariant(s[0]) + (s.Length > 1 ? s.Substring(1) : string.Empty);
    }

    public static string Indent(this string s, int indent)
    {
        var lines = s.Split('\n').Select(x => $"{new string('\t', indent)}{x.Trim('\r')}");
        return string.Join("\r\n", lines);
    }

    public static string ToLiteral(this string s)
    {
        return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(s)).ToFullString();
    }
}
