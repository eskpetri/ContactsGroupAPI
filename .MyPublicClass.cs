
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contactgroupAPIefMySQL
{
    public static class MyPublicClass
    {

        public static Boolean VerifyPassword(String text, String hash) {
            if (BCrypt.Net.BCrypt.Verify(text, hash))
                return true;
            return false;
        }
        public static string EncryptPassword(String pwd) {
            return BCrypt.Net.BCrypt.HashPassword(pwd);
        }
    }
}
