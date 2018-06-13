using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
	public class BasketItem
	{
		public int Id { get; set; }

		public int Count { get; set; }
		public int SizeId { get; set; }
	}
}