using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Entities
{
	public class ProductCard
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ProductType Type { get; set; }

		public Material Material { get; set; }

		public string Description { get; set; }

		public List<string> ImageNames { get; set; }

		public List<SizePrice> Prices { get; set; }

		public string Articul { get; set; }
	}
}
