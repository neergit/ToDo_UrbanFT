using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ToDo_UrbanFt.Controllers;
using ToDo_UrbanFt.dbContext;
using ToDo_UrbanFt.Entities;
using ToDo_UrbanFt.Models;

namespace ToDo_UnitTests;

[TestClass]
public class TaskControllerTests
{
    private static DbContextOptions<TodoListContext> dbContextOptions = new DbContextOptionsBuilder<TodoListContext>()
        .UseInMemoryDatabase("ToDoDB")
        .Options;

    TodoListContext _context;
    private TaskController _controller;

    public TaskControllerTests()
    {
        _context = new TodoListContext(dbContextOptions);
        _context.Database.EnsureCreated();
        _controller = new TaskController(_context);
    }

    [TestMethod]
    public void GetTasksTest()
    {
        IActionResult actionResult = _controller.GetTasks();
        var result = actionResult as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
        Tasks[]? val = result?.Value as Tasks[];
        Assert.AreEqual(5, val?.Count());
    }

    [TestMethod]
    public void GetTaskTest()
    {
        IActionResult actionResult = _controller.GetTask(2);
        var result = actionResult as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
        Tasks? val = result?.Value as Tasks;
        Assert.AreEqual("Title2", val?.Title);
    }

    [TestMethod]
    public void PostTaskTest()
    {
        TaskInput input = new TaskInput { Title = "Testing", Description = "Unit Testing" };
        ActionResult<Tasks> result = _controller.PostTask(input);
        var objResult = result.Result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.Created, objResult?.StatusCode);
        Tasks? val = objResult?.Value as Tasks;
        Assert.AreEqual("Unit Testing", val?.Description);
        Assert.AreEqual(6, val?.Id);

        input.Title = string.Empty;
        ActionResult<Tasks> result1 = _controller.PostTask(input);
        var objResult1 = result1.Result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, objResult1?.StatusCode);
    }

    [TestMethod]
    public void PutTaskTest()
    {
        Tasks input = new Tasks { Id = 3, Title = "Testing", Description = "Unit Testing again", Status = "In Progress" };
        ActionResult<Tasks> result = _controller.PutTask(3, input);
        var objResult = result.Result as ObjectResult;
        Tasks? val = objResult?.Value as Tasks;
        Assert.AreEqual("In Progress", val?.Status);
        Assert.AreEqual("Unit Testing again", val?.Description);

        ActionResult<Tasks> result1 = _controller.PutTask(1, input);
        var objResult1 = result1.Result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, objResult1?.StatusCode);

        input.Description = string.Empty;
        ActionResult<Tasks> result2 = _controller.PutTask(3, input);
        var objResult2 = result2.Result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, objResult2?.StatusCode);
    }

    [TestMethod]
    public void DeleteTest()
    {
        ActionResult<Tasks> result = _controller.DeleteTask(1);
        var objResult = result.Result as ObjectResult;
        Tasks? val = objResult?.Value as Tasks;
        Assert.AreEqual("Title1", val?.Title);
    }

    [TestMethod]
    public void DeleteMultipleTest()
    {
        int[] ids = { 2, 4 };
        ActionResult result = _controller.DeleteMultiple(ids);
        var objResult = result as ObjectResult;
        List<Tasks>? val = objResult?.Value as List<Tasks>;
        Assert.AreEqual(ids.Count(), val?.Count);
    }
}
