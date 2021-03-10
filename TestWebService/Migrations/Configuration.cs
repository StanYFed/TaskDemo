namespace TestWebService.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestWebService.DA.Implementations;
    using TestWebService.DA.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MainContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MainContext context)
        {
            if (!context.Set<User>().Any())
            {
                context.Set<User>().Add(new User
                {
                    FirstName = "Stan",
                    LastName = "Brothers",
                    Email = "first.brother@mail.com"
                });
                context.Set<User>().Add(new User
                {
                    FirstName = "Steve",
                    LastName = "Brothers",
                    Email = "second.brother@mail.com"
                });

                context.SaveChanges();
            }
        }
    }
}
