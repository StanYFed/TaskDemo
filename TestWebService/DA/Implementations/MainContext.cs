namespace TestWebService.DA.Implementations
{
    using System.Data.Entity;
    using TestWebService.DA.Implementations.ContextConfigurations;

    public class MainContext : DbContext
    {
        public MainContext() : base("name=MainContext") { }

        public DbSet<Models.User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}