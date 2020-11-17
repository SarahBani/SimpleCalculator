using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UnitTest
{
    public static class Extensions
    {

        public static bool IsEqual(this IActionResult result, IActionResult otherResult)
        {
            string serializedResult = JsonConvert.SerializeObject(result);
            string serializedOtherResult = JsonConvert.SerializeObject(otherResult);

            return serializedResult.Equals(serializedOtherResult);
        }

    }
}
