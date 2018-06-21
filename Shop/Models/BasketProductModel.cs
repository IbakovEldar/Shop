
namespace Shop.Models
{
	public class BasketProductModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Articul { get; set; }

		public SizePriceModel Price { get; set; }

		public int Count { get; set; }

		public string Image { get; set; }
	}
}