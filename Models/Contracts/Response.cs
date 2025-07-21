namespace Models.Contracts
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = String.Empty;
        public string ErrorMessage { get; set; } = String.Empty;
    }
}