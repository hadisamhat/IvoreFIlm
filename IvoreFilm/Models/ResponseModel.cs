namespace IvoreFilm.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }

        public static ResponseModel ReturnSuccess()
        {
            return new ResponseModel
            {
                Success = true
            };
        }
    }
}