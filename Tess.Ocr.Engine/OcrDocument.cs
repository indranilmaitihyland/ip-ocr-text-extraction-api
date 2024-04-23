using System.Runtime.Serialization;

namespace Tess.Ocr.Engine
{
    /// <summary>
    /// Contains OCR data for a document
    /// </summary>
    public class OcrDocument
    {
        /// <summary>
        /// Constructs a new, empty <see cref="OcrDocument"/>
        /// </summary>
        public OcrDocument()
        {
            Pages = new();
        }
        /// <summary>
        /// Creates a new <see cref="OcrDocument"/> with a single page
        /// </summary>
        /// <param name="page"></param>
        public OcrDocument(OcrPage page)
            : this()
        {
            if (page != null)
            {
                Pages.Add(page);
            }
        }

        /// <summary>
        /// Gets or sets a collection of page data
        /// </summary>
        public OcrPageList Pages { get; set; }
    }

    public class OcrDocumentList : List<OcrDocument> { }
}
