using Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Controllers;
using Xunit;

namespace UnitTest
{
    public class CalculationControllerTest
    {

        #region Properties

        private CalculationController _controller;

        public static IEnumerable<object[]> WithZeroData => new List<object[]>
         {
            new object[] { new CalculationTestModel(new decimal[] { 0, 0, 0 }, 0, 0, 0, null) },
            new object[] { new CalculationTestModel(new decimal[] { 3, 0, 2 }, 5, 1, 0, null) },
        };

        public static IEnumerable<object[]> LargeData => new List<object[]>
        {
            new object[] { new CalculationTestModel(new decimal[] { decimal.MaxValue, decimal.MaxValue }, null, 0, null, 1) },
            new object[] { new CalculationTestModel(new decimal[] { decimal.MinValue, decimal.MinValue }, null, 0, null, 1) },
            new object[] { new CalculationTestModel(new decimal[] { 1, decimal.MaxValue }, null, 1 - decimal.MaxValue, decimal.MaxValue, 0) },
            new object[] { new CalculationTestModel(new decimal[] { 1, decimal.MinValue }, 1 + decimal.MinValue, null, decimal.MinValue, 0) },
            new object[] { new CalculationTestModel(new decimal[] { decimal.MaxValue - 1, 1}, decimal.MaxValue, decimal.MaxValue - 2, decimal.MaxValue - 1, decimal.MaxValue - 1) },
        };

        public static IEnumerable<object[]> NormalData => new List<object[]>
        {
            new object[] { new CalculationTestModel(new decimal[] { 2, 3 },  5, -1, 6, .67M)},
            new object[] { new CalculationTestModel(new decimal[] { 3, 4.2M, 6 }, 13.2M, -7.2M, 75.6M, 0.12M) },
            new object[] { new CalculationTestModel(new decimal[] { -2, -3 }, -5, 1, 6, .67M) },
            new object[] { new CalculationTestModel(new decimal[] { -102, 3, 6 }, -93, -111, -1836, -5.67M) },
        };

        #endregion /Properties

        #region Constructors

        public CalculationControllerTest()
        {
            this._controller = new CalculationController();
        }

        #endregion /Constructors

        #region Facts & Theories

        #region Add        

        [Theory]
        [MemberData(nameof(LargeData))]
        public void Add_LargeNumbers_ReturnsOK_ReturnsBadRequestOverflow(CalculationTestModel data)
        {
            // Arrange
            IActionResult expectedResult;
            if (data.AddOutput != null)
            {
                expectedResult = new OkObjectResult(data.AddOutput);
            }
            else
            {
                expectedResult = Helper.GetBadRequestResult(Constant.Exception_OverFlow);
            }

            //Act
            var actualResult = this._controller.Add(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        [Theory]
        [MemberData(nameof(NormalData))]
        [MemberData(nameof(WithZeroData))]
        public void Add_ReturnsOK(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = new OkObjectResult(data.AddOutput);

            //Act
            var acrualResult = this._controller.Add(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(acrualResult));
        }

        #endregion /Add

        #region Subtract

        [Theory]
        [MemberData(nameof(LargeData))]
        public void Subtract_LargeNumbers_ReturnsOK_ReturnsBadRequestOverflow(CalculationTestModel data)
        {
            // Arrange
            IActionResult expectedResult;
            if (data.SubtractOutput != null)
            {
                expectedResult = new OkObjectResult(data.SubtractOutput);
            }
            else
            {
                expectedResult = Helper.GetBadRequestResult(Constant.Exception_OverFlow);
            }

            //Act
            var actualResult = this._controller.Subtract(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        [Theory]
        [MemberData(nameof(NormalData))]
        [MemberData(nameof(WithZeroData))]
        public void Subtract_ReturnsOK(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = new OkObjectResult(data.SubtractOutput);

            //Act
            var actualResult = this._controller.Subtract(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        #endregion /Subtract

        #region Multiply

        [Theory]
        [MemberData(nameof(LargeData))]
        public void Multiply_LargeNumbers_ReturnsOK_ReturnsBadRequestOverflow(CalculationTestModel data)
        {
            // Arrange
            IActionResult expectedResult;
            if (data.MultiplyOutput != null)
            {
                expectedResult = new OkObjectResult(data.MultiplyOutput);
            }
            else
            {
                expectedResult = Helper.GetBadRequestResult(Constant.Exception_OverFlow);
            }

            //Act
            var actualResult = this._controller.Multiply(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        [Theory]
        [MemberData(nameof(NormalData))]
        [MemberData(nameof(WithZeroData))]
        public void Multiply_ReturnsOK(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = new OkObjectResult(data.MultiplyOutput);

            //Act
            var actualResult = this._controller.Multiply(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        #endregion /Multiply

        #region Divide

        [Theory]
        [MemberData(nameof(WithZeroData))]
        public void Divide_WithZeroData_ReturnsBadRequestDividedByZero(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = Helper.GetBadRequestResult(Constant.Exception_DividedByZero);

            //Act
            var actualResult = this._controller.Divide(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        [Theory]
        [MemberData(nameof(LargeData))]
        public void Divide_LargeNumbers_ReturnsOK_ReturnsBadRequestOverflow(CalculationTestModel data)
        {
            // Arrange
            IActionResult expectedResult;
            if (data.DivideOutput != null)
            {
                expectedResult = new OkObjectResult(data.DivideOutput);
            }
            else
            {
                expectedResult = Helper.GetBadRequestResult(Constant.Exception_OverFlow);
            }

            //Act
            var actualResult = this._controller.Divide(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        [Theory]
        [MemberData(nameof(NormalData))]
        public void Divide_ReturnsOK(CalculationTestModel data)
        {
            // Arrange
            var expectedResult = new OkObjectResult(data.DivideOutput);

            //Act
            var actualResult = this._controller.Divide(data.Input);

            // Assert
            Assert.True(expectedResult.IsEqual(actualResult));
        }

        #endregion /Divide

        #endregion /Facts & Theories

    }
}
