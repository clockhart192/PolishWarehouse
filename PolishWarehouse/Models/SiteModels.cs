namespace PolishWarehouse.Models
{
    public class Response
    {
        public bool WasSuccessful { get; set; }
        public string Message { get; set; }

        public Response(bool wasSuccessful, string message = "")
        {
            WasSuccessful = wasSuccessful;
            Message = message;
        }
    }
}