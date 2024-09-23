namespace Practice.WebAPI.Utility
{
    public interface ISendEmail 
    {
        public Task SendMail(string toMail, string subject, string body);
    }
}