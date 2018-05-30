using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Dal.Interface;

namespace Shop.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductRepository _producyRepository;

		public HomeController(IProductRepository producyRepository)
		{
			_producyRepository = producyRepository;
		}

		public ActionResult Index()
		{
			//return View(new List<ProductItem> {
			//	new ProductItem
			//	{
			//		Name = "Эльдрадо",
			//		Size="120х80",
			//		ImageUrl="/images/1517517233573_815060.jpg"
			//	},
			//	 new ProductItem
			//	{
			//		Name = "Голубая Лагуна",
			//		Size="120х80",
			//		ImageUrl="/images/1517517233573_815060.jpg"
			//	},
			//	  new ProductItem
			//	{
			//		Name = "Нежность",
			//		Size="120х80",
			//		ImageUrl="/images/1517517233573_815060.jpg"
			//	},
			//	   new ProductItem
			//	{
			//		Name = "Этнос",
			//		Size="120х80",
			//		ImageUrl="~/images/1517517233573_815060.jpg"
			//	},
			//});
			return View();
		}

		public ActionResult Catalog()
		{


			return View();
		}

		public ActionResult Contact()
		{


			return View();
		}

		public ActionResult Delivery()
		{
			return View("Delivery");
		}
	}
}