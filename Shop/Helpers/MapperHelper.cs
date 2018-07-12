using Shop.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Helpers
{
	public static class MapperHelper
	{
		

		public static string ImageUrl(this Product product)
		{
			return $"/ProductImages/{product.FileName}";
		}

		public static string GetMaterial(this Product product)
		{
			var material = string.Empty;
			switch(product.Material)
			{
				case Material.Chlopok:
				material = "Хлопок";
				break;
			}
			return material;
		}

		public static string GetMaterial(this Material material)
		{
			var name = string.Empty;
			switch(material)
			{
				case Material.Chlopok:
					name="Хлопок";
					break;
			}
			return name;
		}

		public static string GetMaterial(this ProductCard product)
		{
			var material = string.Empty;
			switch(product.Material)
			{
				case Material.Chlopok:
				material="Хлопок";
				break;
			}
			return material;
		}

		public static string GetPathName(this ProductType type)
		{
			var name = string.Empty;
			switch(type)
			{
				case ProductType.BedClothes:
				name = "Постельное белье";
				break;
				case ProductType.Pillow:
				name = "Подушки";
				break;
			}
			return name;
		}
	}
}