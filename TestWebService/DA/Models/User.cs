namespace TestWebService.DA.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // TODO: Unique?
        public string Email { get; set; }
    }
}