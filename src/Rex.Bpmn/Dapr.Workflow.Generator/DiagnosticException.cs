using Microsoft.CodeAnalysis;
using System;

namespace Rex.Bpmn.Dapr.Workflow.Generator;

internal class DiagnosticException : Exception
{
    public DiagnosticException(DiagnosticDescriptor descriptor, Location location, params object[] messageArgs)
    {
        Diagnostic = Diagnostic.Create(descriptor, location, messageArgs);
    }

    public Diagnostic Diagnostic { get; }
}