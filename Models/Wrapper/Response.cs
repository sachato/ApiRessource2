namespace ApiRessource2.Models.Wrapper
{
    public class Response<T>
    {
        public Response()
        {

        }


        public Response(T data, string[] error = null)
        {
            if (error == null)
            {
                Succeeded = true;
                Errors = error;
            }
            else
            {
                Succeeded = false;
                Errors = error;
            }   
            Message = string.Empty;
            Data = data;
        }


        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
