using Api.Config.Helpers;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace Api.Config
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterMediator(new HierarchicalLifetimeManager())
                .RegisterMediatorHandlers();

            config.DependencyResolver = new UnityResolver(container);

            config.MapHttpAttributeRoutes();
        }
    }
}
