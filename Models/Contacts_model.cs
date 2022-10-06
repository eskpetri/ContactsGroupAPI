using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace contactgroupAPI
{
    public class Contacts
    {
        public int idcontacts { get; set; } = 0;
        public string username { get; set; } = String.Empty;
        public string password { get; set; } = String.Empty;
        public string nickname { get; set; } = String.Empty;
        public string email { get; set; } = String.Empty;
        public string phone { get; set; } = String.Empty;

        public int isadmin {get; set; } = 0;

        internal Database Db { get; set; }

        public Contacts()
        {
            Db = new Database("");  //Just redusing Problems by empty values
        }

        internal Contacts(Database db)
        {
            Db = db;
        }
        internal Contacts(Database db, Contacts b)
        {
            this.idcontacts=b.idcontacts; this.username=b.username; this.password=b.password; this.nickname=b.nickname; this.email=b.email;this.phone=b.phone;
            Db = db;
        }

        public async Task<List<Contacts>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM contacts ;";
            var result=await ReturnAllAsync(await cmd.ExecuteReaderAsync());
            Console.WriteLine(result);
            return result; //await ReturnAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Contacts> FindOneAsync(int idcontacts)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM contacts WHERE idcontacts = @idcontacts";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@idcontacts",
                DbType = DbType.Int32,
                Value = idcontacts,
            });
            var result = await ReturnAllAsync(await cmd.ExecuteReaderAsync());
            //Console.WriteLine(result.Count);
            if(result.Count > 0){
                return result[0];
            }
            else {
                return null;
            }
            //return result.Count > 0 ? result[0] : null;
        }

        public async Task<int> GetContactsidAsync(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT idContacts FROM Contacts WHERE  username = @Contactsname";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Contactsname",
                DbType = DbType.String,
                Value = username,
            });
            var result = ReturnContactsid(await cmd.ExecuteReaderAsync());
            //Console.WriteLine(result.Count);
            if(result > 0){
                return result;
            }
            else {
                return 0;
            }
            //return result.Count > 0 ? result[0] : null;
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM  Contacts ";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }
    

        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText=@"INSERT INTO `contacts`(`username`,`password`,`nickname`,`email`,`phone`)VALUES(@username,@password,@nickname, @email,@phone);";
            BindParams(cmd);
            try
            {
                await cmd.ExecuteNonQueryAsync();
                int lastInsertId = (int) cmd.LastInsertedId; 
                return lastInsertId;
            }
            catch (System.Exception)
            {   
                return 0;
            } 
        }

        public async Task<int> UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            
            cmd.CommandText = @"UPDATE contacts SET username = @username, password = @password, nickname = @nickname, email = @email, phone = @phone WHERE idcontacts = @idcontacts;";
            BindParams(cmd);
            BindId(cmd);
            
            Console.WriteLine("UpdateAsync id="+idcontacts);
            int returnValue=await cmd.ExecuteNonQueryAsync();
            return returnValue;
        }

        public async Task<int> DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            int ups = 0;
            cmd.CommandText = @"DELETE FROM contacts WHERE idcontacts = @idcontacts;";
            BindId(cmd);
            ups = await cmd.ExecuteNonQueryAsync();
            return ups;
        }

        private async Task<List<Contacts>> ReturnAllAsync(DbDataReader reader)
        {
            var posts = new List<Contacts>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Contacts(Db)
                    {
                        idcontacts = reader.GetInt32(0),
                        username = reader.GetString(1),
                        password = reader.GetString(2),
                        nickname = reader.GetString(3),
                        email = reader.GetString(4),
                        phone = reader.GetString(5),
                        isadmin = reader.GetInt32(6)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private int ReturnContactsid(DbDataReader reader)
        {
            int post = 0;  //return zero if no contact id found or error
            try
            {
                reader.Read();
                Console.WriteLine("reader value" +reader.IsDBNull(0)+reader.ToString());
                //if (reader.IsDBNull)
                post = reader.GetInt32(0);
                return post;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Virhe ReturnContactid: "+ex.Message);
                return 0;
            }
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@idcontacts",
                DbType = DbType.Int32,
                Value = idcontacts,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = password,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nickname",
                DbType = DbType.String,
                Value = nickname,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phone",
                DbType = DbType.String,
                Value = phone,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@isadmin",
                DbType = DbType.Int32,
                Value = isadmin,
            });
        }
    }
}