using System.Runtime;

namespace Tess.Ocr.Engine
{
    /// <summary>
    /// Interface for performing OCR operations on a document or file
    /// </summary>
    public interface IOcrEngine : IDisposable
    {
        /// <summary>
        /// Performs recognition on an image given an image stream
        /// </summary>
        /// <param name="imageData">Stream containing image data</param>
        /// <param name="ocrSettings">Set of recognition settings</param>
        /// <returns>Recognized document data</returns>
        Task<OcrDocument> PerformImageRecognitionAsync(Stream imageData, OcrSettings ocrSettings);
        /// <summary>
        /// Initializes the OCR engine
        /// </summary>
        /// <returns>Whether or not the engine was initialized</returns>
        /// <exception cref="InvalidOperationException">The engine could not be initialized, or a license was not found.</exception>
        Task<bool> InitializeAsync();
    }
}
