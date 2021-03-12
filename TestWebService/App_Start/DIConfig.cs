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
                    cfg.CreateMap<User, UserDto>()
                        .ForMember(dst => dst.Id, opts => opts.MapFrom(src => src.Id))
                        .ForMember(dst => dst.FirstName, opts => opts.MapFrom(src => src.FirstName))
                        .ForMember(dst => dst.LastName, opts => opts.MapFrom(src => src.LastName))
                        .ForMember(dst => dst.Email, opts => opts.MapFrom(src => src.Email))
                    .ReverseMap()
                        .ForMember(dst => dst.Id, opts => opts.MapFrom(src => src.Id))
                        .ForMember(dst => dst.FirstName, opts => opts.MapFrom(src => src.FirstName))
                        .ForMember(dst => dst.LastName, opts => opts.MapFrom(src => src.LastName))
                        .ForMember(dst => dst.Email, opts => opts.MapFrom(src => src.Email));
                });

                return new Mapper(config);
            }).SingleInstance();
        }
    }
}