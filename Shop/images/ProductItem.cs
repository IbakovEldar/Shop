using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ProductItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public string ImageUrl { get; set; }
												  
		public string Material { get; set; }

		public string Description { get; set; }
    }
}