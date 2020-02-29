using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Lifetime;

namespace Api.Config.Helpers
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer RegisterMediator(this IUnityContainer container, ITypeLifetimeManager lifetimeManager)
        {
            return container.RegisterType<IMediator, Mediator>(lifetimeManager)
                .RegisterInstance<ServiceFactory>(type =>
                {
                    var enumerableType = type
                        .GetInterfaces()
                        .Concat(new[] { type })
                        .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                    return enumerableType != null
                        ? container.ResolveAll(enumerableType.GetGenericArguments()[0])
                        : container.IsRegistered(type)
                            ? container.Resolve(type)
                            : null;
                });
        }

        public static IUnityContainer RegisterMediatorHandlers(this IUnityContainer container)
        {
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var implementation in assemblyTypes.Where(t => t.GetInterfaces().Any(implementation => IsSubclassOfRawGeneric(typeof(IRequestHandler<,>), implementation))))
            {
                var interfaces = implementation.GetInterfaces();
                foreach (var @interface in interfaces)
                    container.RegisterType(@interface, implementation);
            }

            return container;
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var currentType = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == currentType)
                    return true;

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}