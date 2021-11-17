namespace JSON_CRUD
{
    public class CryptAccess
    {
        public CryptAccess(string password, int saltRounds)
        {
            this.password = password;
            this.saltRounds = saltRounds;
        }

        public string password { get; }
        public int saltRounds { get; }
    }
}
