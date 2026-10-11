using System.Text.Json;
using CG.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CG.Tests.Unit.Exceptions;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_UsesAppExceptionStatusAndDetails()
    {
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var handled = await handler.TryHandleAsync(
            context,
            new NotFoundException("Record was not found."),
            CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        context.Response.ContentType.Should().StartWith("application/json");
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.GetProperty("title").GetString().Should().Be("Not Found");
        document.RootElement.GetProperty("detail").GetString().Should().Be("Record was not found.");
    }

    [Fact]
    public async Task TryHandleAsync_Uses500ForUnexpectedExceptions()
    {
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException("Sensitive internal detail"),
            CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.GetProperty("title").GetString().Should().Be("An unexpected error occurred.");
        document.RootElement.TryGetProperty("detail", out _).Should().BeFalse();
    }
}