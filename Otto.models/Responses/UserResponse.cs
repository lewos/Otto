namespace Otto.models.Responses
{
    public class UserResponse<T>
    {
        public UserResponse(ResponseCode r, string v, T dto)
        {
            res = r;
            msg = v;
            user = dto;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T user { get; set; }

    }
}
