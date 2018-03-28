namespace PolishWarehouse.Models
{
    public class Response
    {
        public bool WasSuccessful { get; set; }
        public string Message { get; set; }
        public object returnObject { get; set; }
        public PolishModel Polish { get; set; }

        public Response(bool wasSuccessful = true, string message = "",object returnobj = null,PolishModel p = null)
        {
            WasSuccessful = wasSuccessful;
            Message = message;
            returnObject = returnobj;
            Polish = p;
        }
    }
}