namespace Otto.orders
{
    public static class ConfigHelper
    {
        public static string GetConnectionString(string val = null)
        {
            var c = "";
            if (string.IsNullOrEmpty(val))
                c = Environment.GetEnvironmentVariable("DATABASE_URL");
            else
                c = val;

            var cred_database = c.Split("@");

            var cred = cred_database[0].Split("//");


            var user = cred[1].Split(":")[0];
            var pass = cred[1].Split(":")[1];


            var database = cred_database[1].Split(":");
            var srv = database[0];

            var server = database[0];
            var strPort = database[1].Split("/")[0];
            var port = Int32.Parse(String.IsNullOrEmpty(strPort) ? "5432" : strPort);
            var db = database[1].Split("/")[1];


            return $"Server={server};Port={port};Database={db};Uid={user};Pwd={pass};";

        }
    }
}
