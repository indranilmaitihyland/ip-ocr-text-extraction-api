namespace Tess.Ocr.Engine
{
    /// <summary>
    /// Contains settings for image recognition request
    /// </summary>
    public class OcrSettings
    {
        /// <summary>
        /// Gets or sets the value of timeout for recognition operations (in seconds)
        /// </summary>
        public uint TimeOut { get; set; } = 60;
    }
}
