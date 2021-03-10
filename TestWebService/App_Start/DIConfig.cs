namespace TestWebService
{
    using Autofac;
    using Autofac.Integration.WebApi;
    using AutoMapper;
    using System;
    using System.Web.Http;
    using TestWebService.DA;
    using TestWebService.DA.Implementations;
    using TestWebService.DA.Models;
    using TestWebService.Models;

    public class DIConfig
    {
        public static void RegisterDependencies(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            RegisterMapper(builder);

            builder.Register<Func<MainContext>>(_ => () => new MainContext()).SingleInstance();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();

            var container = builder.Build();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterMapper(ContainerBuilder builder)
        {
            builder.Register<IMapper>(_ => {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<User, UserDto>().ReverseMap();
                });

                return new Mapper(config);
            }).SingleInstance();
        }
    }
}