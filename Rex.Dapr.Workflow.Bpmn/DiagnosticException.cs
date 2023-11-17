using Microsoft.CodeAnalysis;
using System;

namespace Rex.Dapr.Workflow.Bpmn
{
    internal class DiagnosticException : Exception
    {
        public DiagnosticException(DiagnosticDescriptor descriptor, Location location, params object[] messageArgs)
        {
            Diagnostic = Diagnostic.Create(descriptor, location, messageArgs);
        }

        public Diagnostic Diagnostic { get; }
    }
}