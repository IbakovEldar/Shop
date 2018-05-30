using Shop.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
	public class ProductListModel
	{
		public List<Product> Products { get; set; }

		public int CurrentPage { get; set; }

		public int TotalCount { get; set; }

		public string PathName { get; set; }
	}
}