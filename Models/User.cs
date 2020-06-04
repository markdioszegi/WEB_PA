namespace PA
{
    public class User
    {
        int id { get; set; }
        string username { get; set; }
        string password { get; set; }
        string email { get; set; }
        string role { get; set; }

        public User(int id, string username, string password, string email, string role)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.email = email;
            this.role = role;
        }

        public override string ToString()
        {
            return $"id: {id}\nusername: {username}\npass: {password}\nemail: {email}\nrole: {role}";
        }
    }
}