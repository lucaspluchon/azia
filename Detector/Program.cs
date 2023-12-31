﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using Microsoft.ML.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

// Configure app
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput>()
    .FromFile("MLModel1.mlnet");

builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls("http://*:80");


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Description = "Docs for my API", Version = "v1" });
});
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// Define prediction route & handler
app.MapPost("/predict",
    async (PredictionEnginePool<MLModel1.ModelInput, MLModel1.ModelOutput> predictionEnginePool, HttpRequest request) =>
    {
        var file = request.Form.Files["imageFile"];
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileBytes = ms.ToArray();

        var input = new MLModel1.ModelInput()
        {
            ImageSource = fileBytes,
        };
        
        var result = predictionEnginePool.Predict(input);

        var response = new
        {
            PredictedLabel = result.PredictedLabel,
            Score = result.Score
        };


        return await Task.FromResult(response);
    });

// Run app
app.Run();
