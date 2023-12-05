using Rex.Bpmn.Abstractions;

namespace Rex.Bpmn.Drawing.Test;

public class DiagramToSvgVisitorTest
{
    [Fact]
    public void Test()
    {
        var model = BpmnModel.Load(@"D:\s\github\markrexwinkel\dapr-workflow-bpmn-generator\src\Rex.Bpmn\TestApp\Workflows\LoanApplication.bpmn");
        var diagram = model.Definitions.BpmnDiagrams[0];
        diagram.WriteSvg(model.Definitions, "diagram.svg", new Dictionary<string, int> { ["Flow_13gy3ar"] = 1 });
    }
}