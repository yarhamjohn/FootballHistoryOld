using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Controllers;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class HistoricalPositionControllerTests
    {
        // [Test]
        // public void GetHistoricalPositions_should_return_problem_given_unhandled_exception()
        // {
        //     var mockBuilder = new Mock<IHistoricalPositionBuilder>();
        //     mockBuilder
        //         .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<long>()))
        //         .Throws(new Exception("Something went wrong"));
        //
        //     var controller = new HistoricalPositionController(mockBuilder.Object);
        //     var result = controller.GetHistoricalPositions(1, new[] { 1L });
        //
        //     Assert.That(result, Is.TypeOf<ActionResult<List<HistoricalPositionDto>>>());
        //     Assert.That(result.Result, Is.TypeOf<ObjectResult>());
        //     Assert.That(((ObjectResult)result.Result).StatusCode, Is.EqualTo(500));
        //
        // }
        //
        // [Test]
        // public void GetHistoricalPositions_should_return_message_for_unhandled_error()
        // {
        //     var seasonIds = new long[] {1};
        //
        //     var mockBuilder = new Mock<IHistoricalPositionBuilder>();
        //     mockBuilder
        //         .Setup(x => x.Build(1, 1))
        //         .Throws(new Exception("Unhandled error occurred."));
        //
        //     var controller = new HistoricalPositionController(mockBuilder.Object);
        //     var result = controller.GetHistoricalPositions(1, seasonIds);
        //
        //     mockBuilder.VerifyAll();
        //     result.Should().BeNull();
        //     error.Should().NotBeNull();
        //     error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
        //     error!.Code.Should().Be("UNKNOWN_ERROR");
        // }
        //
        // [Test]
        // public void GetHistoricalPositions_should_return_message_for_handled_error()
        // {
        //     var seasonIds = new long[] {1};
        //
        //     var mockBuilder = new Mock<IHistoricalPositionBuilder>();
        //     mockBuilder
        //         .Setup(x => x.Build(1, 1))
        //         .Throws(new DataInvalidException("Repository data was invalid."));
        //
        //     var controller = new HistoricalPositionController(mockBuilder.Object);
        //     var result = controller.GetHistoricalPositions(1, seasonIds);
        //
        //     mockBuilder.VerifyAll();
        //     result.Should().NotBeNull();
        //     result!.Message.Should().Be("Repository data was invalid.");
        //     result!.Code.Should().Be("DATA_INVALID");
        // }
        //
        // [Test]
        // public void GetHistoricalPositions_should_return_empty_list_given_no_seasonIds()
        // {
        //     var mockBuilder = new Mock<IHistoricalPositionBuilder>();
        //
        //     var controller = new HistoricalPositionController(mockBuilder.Object);
        //     var result = controller.GetHistoricalPositions(1, Array.Empty<long>());
        //
        //     result.Should().BeEquivalentTo(new List<HistoricalPositionDto>());
        // }
        //
        // [Test]
        // public void GetHistoricalPositions_should_return_result()
        // {
        //     var seasonIds = new long[] {1, 2};
        //     var historicalPositionDtoOne = new HistoricalPositionDto(1, 2000, Array.Empty<CompetitionDto>(), null);
        //     var historicalPositionDtoTwo = new HistoricalPositionDto(1, 2000, Array.Empty<CompetitionDto>(), null);
        //
        //     var mockHistoricalPositionOne = new Mock<IHistoricalPosition>();
        //     mockHistoricalPositionOne
        //         .Setup(x => x.ToDto())
        //         .Returns(historicalPositionDtoOne);
        //     
        //     var mockHistoricalPositionTwo = new Mock<IHistoricalPosition>();
        //     mockHistoricalPositionTwo
        //         .Setup(x => x.ToDto())
        //         .Returns(historicalPositionDtoOne);
        //     
        //     var mockBuilder = new Mock<IHistoricalPositionBuilder>();
        //     mockBuilder
        //         .Setup(x => x.Build(1, 1))
        //         .Returns(mockHistoricalPositionOne.Object);
        //     mockBuilder
        //         .Setup(x => x.Build(1, 2))
        //         .Returns(mockHistoricalPositionTwo.Object);
        //
        //     var controller = new HistoricalPositionController(mockBuilder.Object);
        //     var result = controller.GetHistoricalPositions(1, seasonIds);
        //
        //     mockBuilder.VerifyAll();
        //     result.Should().BeEquivalentTo(
        //         new List<HistoricalPositionDto>
        //         {
        //             historicalPositionDtoOne, 
        //             historicalPositionDtoTwo
        //         });
        // }
    }
}