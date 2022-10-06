using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace contactgroupAPI
{
    public class Login
    {
        public string username { get; set; } = String.Empty;
        public string password { get; set; } = String.Empty;

        internal Database Db { get; set; }

        public Login()
        {
            Db = new Database("");
        }

        internal Login(Database db)
        {
            Db = db;
        }


        public async Task<string> GetPassword(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT  password   FROM  contacts  WHERE  username  = @username";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value =  username,
            });
            var result = await ReturnPassword(await cmd.ExecuteReaderAsync());
            return result;
        }

        private async Task<string> ReturnPassword(DbDataReader reader)
        {
            var loginUser = new Login();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new Login(Db)
                    {
                        password = reader.GetString(0)
                    };
                    loginUser=user;
                }
            }

            return loginUser.password;
        }
        
    
    }
}