using Microsoft.AspNetCore.Mvc;

namespace DotNet.OpenVersion.Controllers
{
    public class ExampleController : Controller
    {
        public ExampleController() 
        {
        
        }

        public int? GetRandomNumber(int minNumber, int maxNumber)
        {
            if(minNumber >= maxNumber)
            {
                return null;
            }
            return new Random().Next(minNumber, maxNumber);
        }
    }
}
