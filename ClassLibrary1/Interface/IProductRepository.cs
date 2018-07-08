using Shop.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Interface
{
	public interface IProductRepository
	{
		List<Product> GetProducts(GetProductsRequest request);

		ProductCard GetProductCard(int id);

		List<SizePrice> GetSizes();

		int AddProduct(ProductCard product);
	}
}
