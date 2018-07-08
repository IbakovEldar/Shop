using Dapper;
using Shop.Dal.Entities;
using Shop.Dal.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Shop.Dal.Extensions;

namespace Shop.Dal.Implementation
{

	public class ProductRepository : IProductRepository
	{
		private string DbName { get; set; }
		private string ConnectionString { get; set; }

		public ProductRepository()
		{
			DbName = "Shop";
			var setting = ConfigurationManager.ConnectionStrings[DbName];

			if (setting == null)
				throw new InvalidOperationException($"Connection string Shop not found in ConfigXmlDocument file");
			ConnectionString = setting.ConnectionString;
		}

		private IDbConnection GetConnection()
		{
			return new SqlConnection(ConnectionString);
		}

		public List<Product> GetProducts(GetProductsRequest request)
		{
			using (var connection = GetConnection())
			{
				var query =
					"SELECT p.Id, p.Name, p.Type, p.Material, pp.Price, pph.FileName, p.Description,s.Name as Size, p.Articul" +
					" FROM [dbo].[Products] p  with(nolock) " +
					" JOIN ProductPrice pp with(NOLOCK) on p.Id = pp.ProductId AND pp.Id = (SELECT TOP 1 Id FROM ProductPrice pp2 with(NOLOCK) WHERE pp2.ProductId = p.Id ORDER BY pp2.Price ASC) " +
					" JOIN Sizes s with(NOLOCK) on pp.SizeId =s.Id " +
					" JOIN ProductPhotos pph with(NOLOCK) on pph.ProductId = p.Id AND pph.Id = (SELECT TOP 1 Id FROM ProductPhotos pph2 with(nolock) WHERE pph2.ProductId = p.Id ORDER BY Id ASC) " +
					$" WHERE p.Type={(int) request.Type} ORDER BY p.Id DESC OFFSET {request.Count * request.Page} ROWS  FETCH NEXT {request.Count} ROWS ONLY";
				return connection.Query<Product>(query, commandType: CommandType.Text).ToList();
			}
		}

		public ProductCard GetProductCard(int id)
		{
			using (var connection = GetConnection())
			{
				var query = "SELECT Id, Type,Name,Material,Description, Articul" +
				            $" FROM [dbo].[Products] with(nolock) WHERE Id = {id}";
				var product = connection.Query<ProductCard>(query, commandType: CommandType.Text).FirstOrDefault();

				if (product == null)
				{
					throw new Exception($"Can't found product id ={id}");
				}

				product.ImageNames = connection
					.Query<string>($"SELECT FileName FROM [dbo].[ProductPhotos] with(nolock) where productId = {id}",
						commandType: CommandType.Text)
					.ToList();

				var priceQuery =
					$"SELECT pp.Price,pp.SizeId,s.Name as SizeName FROM [dbo].[ProductPrice] pp with(nolock) JOIN [dbo].[Sizes] s with(nolock) ON pp.SizeId=s.Id WHERE pp.ProductId={id}";
				product.Prices = connection.Query<SizePrice>(priceQuery, commandType: CommandType.Text).ToList();

				return product;
			}
		}

		public List<SizePrice> GetSizes()
		{
			using (var connection = GetConnection())
			{
				var query = "select Id as SizeId, Name as SizeName from [dbo].[Sizes] with(nolock)";
				return connection.Query<SizePrice>(query, commandType: CommandType.Text).ToList();
			}
		}

		public int AddProduct(ProductCard product)
		{
			using (var connection = GetConnection())
			{
				connection.Open();
				using (var trans = connection.BeginTransaction())
				{
					try
					{
						var imagesTable = product.ImageNames.Select(x => new {FileName = x})
							.ToDataTable()
							.AsTableValuedParameter("[dbo].[ImagesTable]");

						var pricesTable = product.Prices.Select(x => new {SizeId = x.SizeId, Price = x.Price})
							.ToDataTable()
							.AsTableValuedParameter("[dbo].[ProductPriceTable]");

						var id = connection.Query<int>("[dbo].[AddProduct]", new
							{
								type = (int) product.Type,
								name = product.Name,
								material = (int) product.Material,
								description = product.Description,
								articul = product.Articul,
								prices = pricesTable,
								images = imagesTable,
							}, commandType: CommandType.StoredProcedure, transaction: trans)
							.First();
						trans.Commit();
						return id;
					}
					catch (Exception ex)
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}
	}
}
