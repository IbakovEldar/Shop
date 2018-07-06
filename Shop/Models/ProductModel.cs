using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
	public class ProductModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Articul { get; set; }

		public string Material { get; set; }

		public string Description { get; set; }

		public List<SizePriceModel> Prices { get; set; }

		public string MainImageName { get; set; }
		public List<string> ImageNames { get; set; }
	}
}