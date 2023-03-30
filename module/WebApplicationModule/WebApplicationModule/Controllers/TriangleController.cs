using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationModule.Controllers
{
    public class TriangleController : Controller
    {
        [HttpGet("task1")]
        public async Task<ActionResult<double>> GetSin(double a, double b)
        {
            double c = Math.Sqrt(a * a + b * b);
            double d = (a + b) / 2;
            return Ok(new { c, d });
        }
        [HttpGet("task2")]
        public async Task<ActionResult<double>> GetSin(int n)
        {
            bool f = true;
            int num1;
            int num2 = 0;
            if (n < 10)
            {
                return BadRequest(400);
            }
            while (n > 0)
            {
                num1 = n % 10;
                if (num1 < num2)
                {
                    f = false;
                }
                num2 = num1;
                n /= 10;
            }
            return Ok(f);
        }
    }
}
