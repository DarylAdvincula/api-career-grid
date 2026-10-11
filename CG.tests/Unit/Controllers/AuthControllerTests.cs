using CG.Controllers;
using CG.DTO.UserAccount;
using CG.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CG.Tests.Unit.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task RegisterAsApplicant_Returns201AndCreatedAccount()
    {
        var request = new UserAccountCreateDto { Email = "applicant@example.com" };
        var expected = new UserAccountDto
        {
            FirstName = "Ava",
            LastName = "Applicant",
            Email = "applicant@example.com"
        };
        var authService = new Mock<IAuthService>();
        authService.Setup(x => x.RegisterAsApplicantAsync(request)).ReturnsAsync(expected);
        var controller = new AuthController(authService.Object);

        var result = await controller.RegisterAsApplicant(request);

        var response = result.Should().BeOfType<ObjectResult>().Subject;
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Value.Should().BeSameAs(expected);
        authService.Verify(x => x.RegisterAsApplicantAsync(request), Times.Once);
    }
}