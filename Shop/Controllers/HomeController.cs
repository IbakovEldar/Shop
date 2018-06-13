using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Dal.Interface;
using Newtonsoft.Json;

namespace Shop.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductRepository _producyRepository;

		public HomeController(IProductRepository producyRepository)
		{
			_producyRepository=producyRepository;
		}

		public ActionResult Index()
		{
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

		public ActionResult GetBasket()
		{
			var cookieBasket = Request.Cookies["basket"]?.Value;
			var basket = new Basket();
			if(!string.IsNullOrEmpty(cookieBasket))
			{
				basket=JsonConvert.DeserializeObject<Basket>(WebUtility.UrlDecode(cookieBasket));
			}

			var model = new List<BasketProductModel>();
			foreach (var product in basket.Products)
			{
				var productEntity = _producyRepository.GetProductCard(product.Id);
				var inputProduct = basket.Products.First(x => x.Id == productEntity.Id);

				var productModel = new BasketProductModel
				{
					Id = productEntity.Id,
					Name = productEntity.Name,
					Articul = productEntity.Articul,
					Count = inputProduct.Count,
					Image = productEntity.ImageNames.First(),
					Description = productEntity.Description
				};
				
				var size = productEntity.Prices.First(x => x.SizeId == inputProduct.SizeId);
				productModel.Price = new SizePrice {Price = size.Price, SizeId = size.SizeId, SizeName = size.SizeName};
				model.Add(productModel);
			}

			return View("OrderBasketView", new BasketModel {Products = model});
		}
	}
}