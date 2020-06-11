namespace PA
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        /* public User(int id, string username, string password, string email, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        } */

        public override string ToString()
        {
            return $"id: {Id}\nusername: {Username}\npass: {Password}\nemail: {Email}\nrole: {Role}";
        }
    }
}