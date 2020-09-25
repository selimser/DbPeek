namespace DbPeek.Models
{
    internal class ParseStoredProcedureResult : BaseResult
    {
        public string Schema { get; set; }
        public string Name { get; set; }
    }
}
