using System;
using System.Collections.Generic;
using System.Drawing;
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
				SizePriceModels = sizes.Select(x => new SizePriceModel
				{
					Price = x.Price,
					SizeId = x.SizeId,
					SizeName = x.SizeName
				}).ToList()
			};
			return View(model);
		}

		public ActionResult GetPriceArea()
		{
			var sizes = _productRepository.GetSizes();
			var model = new AddFormModel
			{
				SizePriceModels = sizes.Select(x => new SizePriceModel
				{
					Price = x.Price,
					SizeId = x.SizeId,
					SizeName = x.SizeName
				}).ToList()
			};
			return PartialView("PriceAria", model);
		}

		public JsonResult Upload()
		{
			var names = new List<string>();
			foreach (string file in Request.Files)
			{
				var upload = Request.Files[file];
				if (upload != null)
				{
					// получаем имя файла
					string fileName = System.IO.Path.GetFileName(upload.FileName);
					var newFileName = Guid.NewGuid() + fileName;
					upload.SaveAs(Server.MapPath("~/TempFolder/" + newFileName));
					names.Add(newFileName);

					//var image = new Bitmap(upload.InputStream);
					//var thumbImage = ScaleImage(image, 100, 100);

					//upload.SaveAs(Server.MapPath("~/ProductImages/" + fileName));
					//thumbImage.Save(Server.MapPath("~/ProductImages/Thumb/" + fileName));

				}
			}
			return Json(names);
		}


		public ActionResult AddOrUpdateProduct(AddFormModel product)
		{
			return new JsonResult();
		}


		static Image ScaleImage(Image source, int width, int height)
		{

			Image dest = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(dest))
			{
				gr.FillRectangle(Brushes.White, 0, 0, width, height);  // Очищаем экран
				gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				float srcwidth = source.Width;
				float srcheight = source.Height;
				float dstwidth = width;
				float dstheight = height;

				if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
				{
					int left = (width - source.Width) / 2;
					int top = (height - source.Height) / 2;
					gr.DrawImage(source, left, top, source.Width, source.Height);
				}
				else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
				{
					float cy = srcheight / srcwidth * dstwidth;
					float top = ((float)dstheight - cy) / 2.0f;
					if (top < 1.0f) top = 0;
					gr.DrawImage(source, 0, top, dstwidth, cy);
				}
				else  // Пропорции исходного изображения более узкие
				{
					float cx = srcwidth / srcheight * dstheight;
					float left = ((float)dstwidth - cx) / 2.0f;
					if (left < 1.0f) left = 0;
					gr.DrawImage(source, left, 0, cx, dstheight);
				}

				return dest;
			}
		}


	}
}