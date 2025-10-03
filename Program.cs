using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ComponentModel;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var endpoint = configuration["AzureOpenAI:Endpoint"];
var apiKey = configuration["AzureOpenAI:ApiKey"];
var deploymentName = configuration["AzureOpenAI:DeploymentName"];

Console.WriteLine("Hello, Microsoft Agent Framework!");

[Description("Gets the current date and time.")]
static string GetDateTime() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

var agent = new AzureOpenAIClient(
    new Uri(endpoint), 
    new AzureKeyCredential(apiKey)
)
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "You are a polite, professional personal assistant who manages tasks, reminders, and schedules, gives clear answers, suggests solutions with confirmation, protects privacy, and asks if unclear.",
                   name: "MyAssistant",
                   description: "A personal assistant for managing tasks and schedules.",
                   tools: [
                       AIFunctionFactory.Create(GetDateTime)
                       ]);

var response = await agent.RunAsync("What is the current date and time?");

Console.WriteLine(response);