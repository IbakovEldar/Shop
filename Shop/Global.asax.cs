using Castle.Windsor;
using Shop.Dal.Windsor;
using Shop.Windsor;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shop
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = new WindsorContainer();
			container
			.InstallControllers()
			.InstallDal();

			var castleControllerFactory = new CastleControllerFactory(container);
			// Добавляем фабрику контроллеров для обработки запросов
			ControllerBuilder.Current.SetControllerFactory(castleControllerFactory);
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
