using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_CRUD
{
    public class CryptAccess
    {
        public CryptAccess(string password, string saltRounds)
        {
            this.password = password;
            this.saltRounds = saltRounds;
        }

        public string password { get; }
        public string saltRounds { get; }
    }
}
