using System;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ValidateModel]
    public class CalculationController : BaseAPIController
    {

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add([FromBody] decimal[] numbers)
        {
            try
            {
                decimal result = Round(numbers.Sum());
                return base.GetSuccessfulResult(result);
            }
            catch (Exception ex)
            {
                return base.GetErrorResult(new CustomException(ex));
            }
        }

        [HttpPost("subtract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Subtract([FromBody] decimal[] numbers)
        {
            try
            {
                decimal result = numbers[0] + numbers.Skip(1).Sum(q => -q);
                return base.GetSuccessfulResult(result);
            }
            catch (Exception ex)
            {
                return base.GetErrorResult(new CustomException(ex));
            }
        }

        [HttpPost("multiply")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Multiply([FromBody] decimal[] numbers)
        {
            try
            {
                decimal result = 1;
                //foreach (decimal item in numbers)
                //{
                //    result *= item;
                //}
                Array.ForEach(numbers, (q) =>
                {
                    result *= q;
                });
                return base.GetSuccessfulResult(Round(result));
            }
            catch (Exception ex)
            {
                return base.GetErrorResult(new CustomException(ex));
            }
        }

        [HttpPost("divide")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Divide([FromBody] decimal[] numbers)
        {
            //if (numbers.Any(q => q.Skip(1).Equals(0))) // Another way to detect divide by zero instead of try/catch
            //{
            //    return base.GetErrorResult(new CustomException(ExceptionKey.DividedByZero));
            //}
            try
            {
                decimal result = numbers[0];
                //if (result.Equals(0))
                //{
                //    return base.GetSuccessfulResult(Round(0));
                //}
                foreach (decimal item in numbers.Skip(1))
                {
                    result /= item;
                }
                return base.GetSuccessfulResult(Round(result));
            }
            catch (Exception ex)
            {
                return base.GetErrorResult(new CustomException(ex));
            }
        }

        private decimal Round(decimal number)
        {
            return Math.Round(number, 2);
        }

    }
}
