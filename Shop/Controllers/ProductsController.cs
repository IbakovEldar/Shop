using System.Linq;
using Shop.Dal.Entities;
using Shop.Dal.Interface;
using Shop.Helpers;
using Shop.Models;
using System.Web.Mvc;

namespace Shop.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductRepository _productRepository;

		public ProductsController(IProductRepository productRepository)
		{
			_productRepository=productRepository;
		}

		// GET: Product
		public ActionResult Products(ProductType? type, int? page, int? count)
		{
			//			var cookie = Request.Cookies[0].Value;
			var productType = type.GetValueOrDefault(ProductType.BedClothes);
			var products = _productRepository.GetProducts(new GetProductsRequest
			{
				Type=productType,
				Page=page.GetValueOrDefault(0),
				Count=count.GetValueOrDefault(6)
			});

			if(Request.IsAjaxRequest())
			{
				return PartialView("ProductListPartialView", new ProductListModel { Products=products, PathName=productType.GetPathName() });
			}

			return View(new ProductListModel { Products=products, PathName=productType.GetPathName() });
		}

		public ActionResult Product(int id)
		{
			var product = _productRepository.GetProductCard(id);

			return PartialView("Product", new ProductModel
			{
				Id = id,
				Name = product.Name,
				Material = product.GetMaterial(),
				MainImageName = product.ImageNames.First(), //"img_1.jpg",
				ImageNames = product.ImageNames, //new System.Collections.Generic.List<string> { "img_1.jpg", "img_2.jpg" },
				Prices =
					product.Prices.Select(x => new Models.SizePriceModel {SizeId = x.SizeId, SizeName = x.SizeName, Price = x.Price})
						.ToList()
				//Prices=new System.Collections.Generic.List<Models.SizePrice> {
				//	new Models.SizePrice { SizeId=1, SizeName="1,5 спальный", Price=1500 },
				//	new Models.SizePrice { SizeId=1, SizeName="2-x спальный", Price=3000 }
				//}
			});
		}

		public ActionResult AddForm()
		{
			var sizes = _productRepository.GetSizes();
			var model = new AddFormModel
			{
				Sizes = sizes.Select(x => new SizePriceModel
				{
					Price = x.Price,
					SizeId = x.SizeId,
					SizeName = x.SizeName
				}).ToList()
			};
			return View(model);
		}
	}
}