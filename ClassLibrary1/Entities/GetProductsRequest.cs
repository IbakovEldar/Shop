using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Entities
{
	public class GetProductsRequest
	{
		public ProductType Type { get; set; }

		public int Page { get; set; }

		public int Count { get; set; }
	}
}
