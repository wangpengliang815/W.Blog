namespace DotnetCore.Common.Helper
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public static class AutofacHelper
    {
        private static readonly object locker = new object();

        static AutofacHelper() { }

        public static void Begin()
        {
            lock (locker)
            {
                if (Builder != null)
                    return;

                Builder = new ContainerBuilder();
            }
        }

        public static ContainerBuilder Builder { get; private set; }

        public static IContainer Container { get; private set; }

        public static void Register<ServiceType, IServiceType>()
        {
            Builder.RegisterType<ServiceType>().As<IServiceType>();
        }

        public static void RegisterSington<ServiceType, IServiceType>()
        {
            Builder.RegisterType<ServiceType>().As<IServiceType>().SingleInstance();
        }

        public static void RegisterNamed<ServiceType, IServiceType>(string name)
        {
            Builder.RegisterType<ServiceType>().Named<IServiceType>(name);
        }

        public static void RegisterNamedSington<ServiceType, IServiceType>(string name)
        {
            Builder.RegisterType<ServiceType>().Named<IServiceType>(name).SingleInstance();
        }

        public static void Registers(params string[] assemblyNames)
        {
            Assembly[] assemblies = LoadAssemblies(assemblyNames);
            Builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }

        public static void RegistersPerDependency(params string[] assemblyNames)
        {
            Assembly[] assemblies = LoadAssemblies(assemblyNames);
            Builder.RegisterAssemblyTypes(assemblies).InstancePerDependency();
        }

        public static void RegisterSington(params string[] assemblyNames)
        {
            Assembly[] assemblies = LoadAssemblies(assemblyNames);
            Builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces().SingleInstance();
        }

        private static Assembly[] LoadAssemblies(params string[] assemblyNames)
        {
            if (assemblyNames == null || !assemblyNames.Any())
            {
                return null;
            }
            else
            {
                Assembly[] assemblies = assemblyNames.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p =>
                {
                    return Assembly.Load(p);
                }).ToArray();
                return assemblies;
            }
        }

        public static IServiceType Reslove<IServiceType>()
        {
            return Container.Resolve<IServiceType>();
        }

        public static void RegisterType(Type type)
        {
            Builder.RegisterType(type);
        }

        public static void RegisterType<T>(Func<T> func)
        {
            Builder.Register<T>(c => { return func(); });
        }

        public static void RegisterType(params string[] assemblyNames)
        {
            Assembly[] assemblies = LoadAssemblies(assemblyNames);
            Builder.RegisterAssemblyTypes(assemblies);
        }

        public static object Reslove(Type type)
        {
            return Container.Resolve(type);
        }

        //public static void RegisterControllers(params string[] controllerAssemblies)
        //{
        //    var assemblies = LoadAssemblies(controllerAssemblies);
        //    Builder.RegisterControllers(assemblies);
        //}

        //public static void RegisterHttpControllers(params string[] controllerAssemblies)
        //{
        //    var assemblies = LoadAssemblies(controllerAssemblies);
        //    Builder.RegisterApiControllers(assemblies);
        //}

        public static void End()
        {
            Container = Builder.Build();
        }

        public static void Populate(IServiceCollection services)
        {
            Builder.Populate(services);
        }

        public static IServiceProvider CreateResolver()
        {
            return new AutofacServiceProvider(Container);
        }
    }

    //public interface IServiceGetter
    //{
    //    T GetByName<T>(string name);
    //}

    //public class ServiceGetter : IServiceGetter
    //{
    //    public T GetByName<T>(string name)
    //    {
    //        return AutofacDependencyResolver.Current.RequestLifetimeScope.ResolveNamed<T>(name);
    //    }
    //}
}
