namespace Tess.Ocr.Engine
{
    public class OcrPage
    {
        public OcrPage()
        {

        }

        public OcrPage(Words ocrWords, long pageId)
        {
            OcrWords = ocrWords;
            PageID = pageId;
        }

        public Words OcrWords { get; set; }
        public long PageID { get; set; }
        /// <summary>
        /// The correction rotation angle found by the OCR engine, if null no angle was detected.
        /// Possible values are 0, 90, 180, 270.
        /// Rotate clockwise using this value to have OCR data in the correct position.
        /// </summary>
        public double CorrectionRotationAngle { get; set; }

        /// <summary>
        /// The skew value after correction rotation angle has been applied.
        /// If skew is positive rotate counterclockwise with the absolute skew value to have data in correct position.
        /// If skew is negative rotate clockwise with the absolute skew value to have data in correct position.
        /// </summary>
        public double Skew { get; set; }

        public override string ToString()
        {
            return $"Page {PageID}";
        }
    }

    public class OcrPageList : List<OcrPage> { }
}
