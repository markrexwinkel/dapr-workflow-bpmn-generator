﻿namespace Rex.Dapr.Workflow.Bpmn;

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
}