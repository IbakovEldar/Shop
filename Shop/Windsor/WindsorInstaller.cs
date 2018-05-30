using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.Linq;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Reflection;
using System.Web.Mvc;

namespace Shop.Windsor
{
	public static class WindsorInstaller
	{
		public static IWindsorContainer InstallControllers(this IWindsorContainer container)
		{
			// регистрируем каждый контроллер по отдельности
			var controllers = Assembly.GetExecutingAssembly()
				.GetTypes().Where(x => x.BaseType == typeof(Controller)).ToList();
			foreach (var controller in controllers)
			{
				container.Register(Component.For(controller).LifestylePerWebRequest());
			}
			return container;
		}
	}
}