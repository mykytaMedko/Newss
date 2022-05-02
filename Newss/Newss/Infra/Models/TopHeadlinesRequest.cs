using Newss.Infra.Constants;

namespace Newss.Infra.Models
{
    public class TopHeadlinesRequest
    {
        public string Q { get; set; }
        public List<string> Sources = new List<string>();
        public Categories? Category { get; set; }
        public Languages? Language { get; set; }
        public Countries? Country { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
