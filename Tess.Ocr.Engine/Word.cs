namespace Tess.Ocr.Engine
{
    public class Word
    {
        public long ID { get; set; }
        public long Page { get; set; }
        public long Line { get; set; }
        public long Left { get; set; }
        public long Top { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public long FontHeight { get; set; }
        public long BaseLine { get; set; }
        public string Text { get; set; }
        public double Confidence { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }

    public class Words : List<Word>
    {
        public Words()
        {
        }
        public Words(IEnumerable<Word> words)
        {
            base.AddRange(words);
        }
        public long WordCount { get; set; }
    }
}
