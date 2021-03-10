namespace TestWebService.DA.Implementations.ContextConfigurations
{
    using System.Data.Entity.ModelConfiguration;
    using TestWebService.DA.Models;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users").HasKey(en => en.Id);

            Property(en => en.FirstName).IsRequired().HasMaxLength(Consts.STRING_LEN);
            Property(en => en.LastName).IsRequired().HasMaxLength(Consts.STRING_LEN);
            Property(en => en.Email).IsRequired().HasMaxLength(Consts.EMAIL_LEN);
        }
    }
}