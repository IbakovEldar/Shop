using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shop.Dal.Implementation;
using Shop.Dal.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Windsor
{
	public static class DalInstall
	{
		public static IWindsorContainer InstallDal(this IWindsorContainer container)
		{
			container.Register
			(Component.For<IProductRepository>()
			.ImplementedBy<ProductRepository>()
			.LifestyleSingleton());

			return container;
		}
	}
}
