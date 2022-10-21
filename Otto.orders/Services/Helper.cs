namespace Otto.orders.Services
{
    public static class Helper
    {
        public static string GetCodeFromRequest(HttpRequest request)
        {
            try
            {
                var query = "";
                if (request.QueryString.HasValue)
                    query = request.QueryString.Value;

                var code = "";
                if (query.Contains("code") && query.Contains("state"))
                    code = query.Split('&')[0].Split('=')[1];
                else if (query.Contains("code") && query.Contains("&") && !query.Contains("state"))
                    code = query.Split('&')[0].Split('=')[1];
                else if (query.Contains("code") && !query.Contains("&") && !query.Contains("state"))
                {
                    code = query.Split('=')[1];
                }

                return code;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex:{ex}");
                return "";
            }

        }
    }
}
