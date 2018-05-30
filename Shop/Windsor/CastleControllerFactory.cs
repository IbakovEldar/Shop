using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shop.Windsor
{
	public class CastleControllerFactory: DefaultControllerFactory
	{
		//контейнер
		public IWindsorContainer Container { get; protected set; }

		public CastleControllerFactory(IWindsorContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			this.Container = container;
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null)
			{
				return null;
			}
			// получаем запрошенный контроллер от Castle
			return Container.Resolve(controllerType) as IController;
		}

		// освобождаем контроллер
		public override void ReleaseController(IController controller)
		{
			var disposableController = controller as IDisposable;
			if (disposableController != null)
			{
				disposableController.Dispose();
			}

			// информируем ioc-контейнер, что контроллер нам больше не нужен
			Container.Release(controller);
		}
	}
}