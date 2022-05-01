using NewssCore.Constants;

namespace NewssCore.Models
{
    public class Error
    {
        public ErrorCodes Code { get; set; }
        public string Message { get; set; }
    }
}
