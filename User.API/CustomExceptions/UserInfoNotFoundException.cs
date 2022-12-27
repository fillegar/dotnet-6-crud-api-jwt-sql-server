namespace User.API.CustomExceptions
{
    public class UserInfoNotFoundException : Exception
    {
        public UserInfoNotFoundException(string message) : base(message) { }
    }
}
