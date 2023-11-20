using Dapr.Workflow;
using System.Diagnostics;

//#if DEBUG
//Debugger.Launch();
//#endif

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDaprClient();
builder.Services.AddDaprWorkflowClient();
builder.Services.AddLoanApplicationWorkflow();

var app = builder.Build();

app.MapControllers();
app.Run();