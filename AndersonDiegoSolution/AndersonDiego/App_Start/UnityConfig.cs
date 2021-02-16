using AndersonDiego.Infra.Handlers;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Infra.Repositories;
using AndersonDiego.Models;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace AndersonDiego
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<Response, Response>();
            container.RegisterType<ResponseError, ResponseError>();
            container.RegisterType<HandlerLogin, HandlerLogin>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}