namespace TestWebService.Controllers
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using TestWebService.Controllers.Extensions;
    using TestWebService.DA;
    using TestWebService.DA.Models;
    using TestWebService.Models;

    public class UsersController : ApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Users
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await this.userRepository.GetAll();
            var retVal = this.mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return this.Ok(retVal);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(long id)
        {
            User user = await this.userRepository.GetById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            var retVal = this.mapper.Map<User, UserDto>(user);
            return this.Ok(retVal);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var dbUser = this.mapper.Map<UserDto, User>(user);
            await this.userRepository.Save(dbUser);
            return this.NoContent();
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(long id, UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return this.BadRequest();
            }

            var dbUser = this.mapper.Map<UserDto, User>(user);

            try
            {
                await this.userRepository.Save(dbUser);
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(long id)
        {
            await this.userRepository.DeleteById(id);
            return this.Ok();
        }
    }
}