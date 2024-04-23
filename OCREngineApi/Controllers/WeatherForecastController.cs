using Microsoft.AspNetCore.Mvc;
using System.Text;
using Tess.Ocr.Engine;
using Tess.Ocr.Engine.Implementations;

namespace OCREngineApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            TesseractOcrEngine engine = new TesseractOcrEngine();
            using FileStream fs = new FileStream("C:\\Indranil\\IPSprint\\Apr2024\\testdata\\1.tif", FileMode.Open);

            bool isSuccess = await engine.InitializeAsync();
            StringBuilder stringBuilder = new StringBuilder();

            if(isSuccess)
            {
                OcrDocument document = await engine.PerformImageRecognitionAsync(fs, new Tess.Ocr.Engine.OcrSettings { TimeOut = 60 });
                if (document != null)
                {
                    foreach(Word word in document.Pages[0].OcrWords)
                    {
                        stringBuilder.Append($"word - {word.Text} and cordinate(x,y,height,width) - {word.Left},{word.Top},{word.Height},{word.Width}");
                        stringBuilder.Append(Environment.NewLine);
                    }
                }
            }


            return stringBuilder.ToString();
                ;
        }
    }
}
