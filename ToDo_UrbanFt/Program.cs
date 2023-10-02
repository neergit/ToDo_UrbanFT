using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using ToDo_UrbanFt.dbContext;
using ToDo_UrbanFt.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoListContext>(options => options.UseInMemoryDatabase("ToDoDb"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("https://localhost:7135")
            .AllowAnyMethod()
            .WithHeaders("content-type");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(error => error.Run(async context =>
{
    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
    var contextRequest = context.Features.Get<IHttpRequestFeature>();
    if(contextFeature is not null)
    {
        var errorString = new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = contextFeature.Error.Message,
            Path = contextRequest.Path
        }.ToString();

        await context.Response.WriteAsync(errorString);
    }
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();

