using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Dal.Entities;

namespace Shop.Models
{
	public class AddFormModel
	{
		public int? Id { get; set; }
		public string Name { get; set; }

		public string Articul { get; set; }

		public string Description { get; set; }

		public ProductType Type { get; set; }

		public Material Material { get; set; }

		public List<string> Images { get; set; }

		public List<SizePriceModel> SizePriceModels { get; set; }

		public List<SizePriceModel> Sizes { get; set; }
	}
}