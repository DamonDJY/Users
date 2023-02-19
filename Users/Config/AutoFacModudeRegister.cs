using Autofac;
using System.Reflection;

namespace Users.WebApi.Config
{
    public class AutoFacModudeRegister:Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            Assembly interfaceAssembly = Assembly.Load("Interface");
            Assembly serviceAssembly = Assembly.Load("Users.Infrastructure");
            builder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();
        }
    }
}
