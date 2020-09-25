namespace DbPeek.Models
{
    internal class BaseResult
    {
        public ErrorData Error { get; set; }
        public bool Success => Error == null;
    }
}
