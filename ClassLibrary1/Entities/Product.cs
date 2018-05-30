using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Entities
{
	public class Product
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ProductType Type { get; set; }

		public Material Material { get; set; }

		public float Price { get; set; }

		public string FileName { get; set; }

		public string Description { get; set; }

		public string Size { get; set; }   

		public string Articul { get; set; }
	}
}
