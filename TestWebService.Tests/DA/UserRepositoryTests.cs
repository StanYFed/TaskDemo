namespace TestWebService.Tests.DA
{
    using NUnit.Framework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using TestWebService.DA;
    using TestWebService.DA.Implementations;
    using TestWebService.DA.Models;

    public class UserRepositoryTests
    {
        private const string CONNECTION = "Server=(localdb)\\mssqllocaldb;Database=DbForTests;Trusted_Connection=True;MultipleActiveResultSets=true";
        private Func<MainContext> dbContextFactory;

        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());

            this.dbContextFactory = () => new MainContext(CONNECTION);
            using (var dbCtx = this.dbContextFactory())
            {
                dbCtx.Database.CreateIfNotExists();

                var configuration = new Migrations.Configuration();
                var migrator = new DbMigrator(configuration, dbCtx);
                migrator.Update();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var dbCtx = this.dbContextFactory())
            {
                dbCtx.Database.Delete();
            }
        }

        [Test]
        public async Task Should_ReturnAll()
        {
            var repo = new UserRepository(this.dbContextFactory);
            var allUser = await repo.GetAll();

            Assert.That(allUser, Is.Not.Null);
            Assert.That(allUser.Any(), Is.True);
        }

        [Test]
        public async Task Should_CRUD()
        {
            var repo = new UserRepository(this.dbContextFactory);

            var newUser = new User
            {
                FirstName = "User",
                LastName = "Test",
                Email = "UserTest@email.com"
            };

            var createdUser = await repo.Save(newUser);
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.Id, Is.Not.EqualTo(0));
            AssertUsersEqual(createdUser, newUser);

            var createdUserFromDb = await repo.GetById(newUser.Id);
            Assert.That(createdUserFromDb, Is.Not.Null);
            AssertUsersEqual(newUser, createdUserFromDb);

            var updateUser = new User
            {
                Id = createdUserFromDb.Id,
                FirstName = "User1",
                LastName = "Test1",
                Email = "UserTest1@email.com"
            };

            var updatedUser = await repo.Save(updateUser);
            Assert.That(updatedUser, Is.Not.Null);
            AssertUsersEqual(updateUser, updatedUser);

            var updatedUserFromDb = await repo.GetById(newUser.Id);
            Assert.That(updatedUserFromDb, Is.Not.Null);
            AssertUsersEqual(updatedUserFromDb, updateUser);

            await repo.DeleteById(newUser.Id);
            var deletedUser = await repo.GetById(newUser.Id);
            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public async Task Should_Not_UpdateDeleted()
        {
            var repo = new UserRepository(this.dbContextFactory);
            var createdUserId = await repo.CreateTestUser();

            await repo.DeleteById(createdUserId);

            var updateUserFirst = new User
            {
                Id = createdUserId,
                FirstName = "User1",
                LastName = "Test",
                Email = "UserTest@email.com"
            };

            Assert.That(async () => await repo.Save(updateUserFirst), Throws.InstanceOf<NotFoundException>());
        }

        [Test]
        public async Task Should_DeleteDeleted()
        {
            var repo = new UserRepository(this.dbContextFactory);
            var createdUserId = await repo.CreateTestUser();

            await repo.DeleteById(createdUserId);
            await repo.DeleteById(createdUserId);

            Assert.Pass();
        }

        private void AssertUsersEqual(User first, User second)
        {
            if (Equals(first, second)) { return; }

            Assert.That(first.FirstName, Is.EqualTo(second.FirstName));
            Assert.That(first.LastName, Is.EqualTo(second.LastName));
            Assert.That(first.Email, Is.EqualTo(second.Email));
        }
    }

    public static class UserRepositoryTestExtensions
    {
        public static async Task<long> CreateTestUser(this UserRepository userRepository)
        {
            var newUser = new User
            {
                FirstName = "User",
                LastName = "Test",
                Email = "UserTest@email.com"
            };

            var createdUser = await userRepository.Save(newUser);
            return createdUser.Id;
        }
    }
}