using Microsoft.CodeAnalysis;
using System;

namespace Rex.Bpmn.Dapr.Workflow.Generator;

internal class DiagnosticException(DiagnosticDescriptor descriptor, Location location, params object[] messageArgs) : Exception
{
    public Diagnostic Diagnostic { get; } = Diagnostic.Create(descriptor, location, messageArgs);
}