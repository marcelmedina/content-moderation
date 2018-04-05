namespace ContentModeration.Models
{

    public class OcrResponse
    {
        public Metadata[] Metadata { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
        public Candidate[] Candidates { get; set; }
        public OcrStatus Status { get; set; }
        public string TrackingId { get; set; }
    }

    public class OcrStatus
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public object Exception { get; set; }
    }

    public class Metadata
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Candidate
    {
        public string Text { get; set; }
        public int Confidence { get; set; }
    }


}
