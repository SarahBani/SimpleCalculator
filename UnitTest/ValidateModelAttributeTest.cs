using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using WebAPI.Filters;
using Xunit;

namespace UnitTest
{
    public class ValidateModelAttributeTest
    {

        #region Properties

        public static IEnumerable<object[]> NullOrEmptyData => new List<object[]> {
            new object[] { new CalculationTestModel(null) },
            new object[] { new CalculationTestModel(new decimal[] { }) },
        };

        public static IEnumerable<object[]> NormalData => new List<object[]>
        {
            new object[] { new CalculationTestModel(new decimal[] { 2, 3 })},
            new object[] { new CalculationTestModel(new decimal[] { 3, 4.2M, 6 }) },
            new object[] { new CalculationTestModel(new decimal[] { -2, -3 }) },
            new object[] { new CalculationTestModel(new decimal[] { -102, 3, 6 }) },
        };

        #endregion /Properties

        #region Facts & Theories

        [Fact]
        public void OnActionExecuting_NoActionArguments_ReturnsBadRequestNullInput()
        {
            // Arrange
            var expectedResult = Helper.GetBadRequestResult(Constant.Exception_NullInput);
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);
            var sut = new ValidateModelAttribute();

            //Act
            sut.OnActionExecuting(context);

            //Assert
            Assert.True(expectedResult.IsEqual(context.Result));
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyData))]
        public void OnActionExecuting_NullOrEmptyArray_ReturnsBadRequestEmptyArray(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = Helper.GetBadRequestResult(Constant.Exception_EmptyArray);

            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>() { { "numbers", data.Input } },
                new Mock<Controller>().Object);

            var sut = new ValidateModelAttribute();

            //Act
            sut.OnActionExecuting(context);

            //Assert
            Assert.True(expectedResult.IsEqual(context.Result));
        }

        [Theory]
        [MemberData(nameof(NormalData))]
        public void OnActionExecuting_ReturnsOK(CalculationTestModel data)
        {
            // Arrange
            IActionResult expectedResult = null;

            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>() { { "numbers", data.Input } },
                new Mock<Controller>().Object);

            var sut = new ValidateModelAttribute();

            //Act
            sut.OnActionExecuting(context);

            //Assert
            Assert.True(expectedResult.IsEqual(context.Result));
        }

        #endregion /Facts & Theories

    }
}
