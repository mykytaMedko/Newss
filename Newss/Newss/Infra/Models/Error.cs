using Newss.Infra.Constants;

namespace Newss.Infra.Models
{
    public class Error
    {
        public ErrorCodes Code { get; set; }
        public string Message { get; set; }
    }
}
