using Dapr.Workflow;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDaprClient();
builder.Services.AddDaprWorkflowClient();
builder.Services.AddBpmnWorkflows();
builder.Services.AddLoanApplicationWorkflow();

var app = builder.Build();

app.MapControllers();
app.Run();