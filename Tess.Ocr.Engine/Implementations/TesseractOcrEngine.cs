using System.Buffers;
using System.Text.RegularExpressions;
using Tesseract;

namespace Tess.Ocr.Engine.Implementations
{
    public class TesseractOcrEngine : IOcrEngine
    {
        private TesseractEngine _engine;
        private readonly SemaphoreSlim _throttler = new(1, 1);
        public Task<bool> InitializeAsync()
        {
            const string datapath = "C:\\Indranil\\IPSprint\\Apr2024\\ConfigurableRedaction\\tessdata\\tessdata";
            _engine = new TesseractEngine(datapath, "eng", EngineMode.Default);
            return Task.FromResult(true);
        }

        public async Task<OcrDocument> PerformImageRecognitionAsync(Stream imageData, OcrSettings ocrSettings)
        {
            Words words;
            await _throttler.WaitAsync();
            using Pix image = await GetPixAsync(imageData);
            using Pix deSkewedImage = image.Deskew(out Scew skew);
            using Page page = _engine.Process(deSkewedImage);
            {
                words = new(GetOcrMatchingWords(page));
            }
            _throttler.Release();
            return new OcrDocument(new OcrPage(words, 1));
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        private static IEnumerable<Word> GetOcrWords(Page page)
        {
            using ResultIterator iter = page.GetIterator();
            iter.Begin();
            int wordId = 0;
            int line = 0;
            do
            {
                do
                {
                    Word w = new();
                    float confidence = iter.GetConfidence(PageIteratorLevel.Word) / 100;
                    if (iter.TryGetBaseline(PageIteratorLevel.TextLine, out Rect baseline))
                        w.BaseLine = baseline.Y1;
                    if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out Rect bounds))
                    {
                        w.Left = bounds.X1;
                        w.Top = bounds.Y1;
                        w.Width = bounds.Width;
                        w.Height = bounds.Height;
                    }
                    w.Text = iter.GetText(PageIteratorLevel.Word);
                    w.Confidence = confidence;
                    w.Line = line;
                    w.ID = wordId++;
                    yield return w;
                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                line++;
            } while (iter.Next(PageIteratorLevel.TextLine));
        }


        private static IEnumerable<Word> GetOcrMatchingWords(Page page, string regex= "[\\s]*[Dd][Oo][Bb]+")
        {
            using ResultIterator iter = page.GetIterator();
            iter.Begin();
            int wordId = 0;
            int line = 0;
            do
            {
                do
                {
                    Word w = new();
                    float confidence = iter.GetConfidence(PageIteratorLevel.Word) / 100;
                    if (iter.TryGetBaseline(PageIteratorLevel.TextLine, out Rect baseline))
                        w.BaseLine = baseline.Y1;
                    if (iter.TryGetBoundingBox(PageIteratorLevel.Block, out Rect bounds))
                    {
                        w.Left = bounds.X1;
                        w.Top = bounds.Y1;
                        w.Width = bounds.Width;
                        w.Height = bounds.Height;
                    }
                    string text = iter.GetText(PageIteratorLevel.Block);
                    w.Text = text;
                    if (!string.IsNullOrEmpty(regex) && Regex.IsMatch(text, regex))
                    {
                        do
                        {
                            Word w1 = new();
                            float confidence1 = iter.GetConfidence(PageIteratorLevel.Word) / 100;
                            if (iter.TryGetBaseline(PageIteratorLevel.TextLine, out Rect baseline1))
                                w1.BaseLine = baseline1.Y1;
                            if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out Rect bounds1))
                            {
                                w1.Left = bounds1.X1;
                                w1.Top = bounds1.Y1;
                                w1.Width = bounds1.Width;
                                w1.Height = bounds1.Height;
                            }
                            w1.Text = iter.GetText(PageIteratorLevel.Word);
                            w1.Confidence = confidence1;
                            w1.Line = line;
                            w1.ID = wordId++;
                        }
                        while (iter.Next(PageIteratorLevel.Word));
                    }
                    w.Confidence = confidence;
                    w.Line = line;
                    w.ID = wordId++;
                    yield return w;
                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Block));
                line++;
            } while (iter.Next(PageIteratorLevel.Block));
        }

        private static async Task<Pix> GetPixAsync(Stream imageStream)
        {
            byte[] buffer = new byte[4 * 1024];
            byte[] allBytes = ArrayPool<byte>.Shared.Rent(25 * 1024 * 1024);
            try
            {
                int bytesRead = 0;
                int totalBytesRead = 0;
                while ((bytesRead = await imageStream.ReadAsync(buffer)) > 0)
                {
                    Buffer.BlockCopy(buffer, 0, allBytes, totalBytesRead, bytesRead);
                    totalBytesRead += bytesRead;
                }
                return Pix.LoadFromMemory(allBytes, totalBytesRead);
            }
            finally
            {

                ArrayPool<byte>.Shared.Return(allBytes);
            }
        }
    }
}
