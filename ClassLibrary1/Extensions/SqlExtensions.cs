using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Shop.Dal.Extensions
{
	public static class SqlExtensions
	{


		public static string GetSubstring(this string text, int count = 250)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;
			var size = Math.Min(text.Length, count);
			return text.Substring(0, size);
		}

		/// <summary>
		/// Converts the sequence of TObject objects into DataTable.
		/// </summary>
		/// <typeparam name="TObject">The type of objects to convert.</typeparam>
		/// <param name="source">The source sequence of objects.</param>
		/// <returns>The converted DataTable.</returns>
		public static DataTable ToDataTable<TObject>(this IEnumerable<TObject> source)
		{
			var result = new DataTable();

			var properties = typeof(TObject).GetProperties();

			foreach (var property in properties)
				result.Columns.Add(property.Name,
					property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType);

			foreach (var item in source)
				result.Rows.Add(properties.Select(property =>
						{
							var value = property.GetValue(item);
							return value ?? DBNull.Value;
						}
					)
					.ToArray());

			return result;
		}

		/// <summary>
		/// Конвертирует перечисление в таблицу DataTable и позвляет установить свойства для игнорирования.
		/// </summary>
		/// <typeparam name="TObject">Тип перечисления для конвертации.</typeparam>
		/// <param name="source">Перечисление для конвертации..</param>
		/// <returns>Конвертированная таблица DataTable.</returns>
		public static DataTable ToDataTable<TObject>(this IEnumerable<TObject> source, string[] ignoreProperties)
		{
			var result = new DataTable();

			var properties = typeof(TObject).GetProperties()
				.Where(property => !ignoreProperties.Contains(property.Name))
				.ToList();

			foreach (var property in properties)
				result.Columns.Add(property.Name,
					property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType);

			foreach (var item in source)
				result.Rows.Add(properties.Select(property =>
						{
							var value = property.GetValue(item);
							return value ?? DBNull.Value;
						}
					)
					.ToArray());

			return result;
		}

		public static SqlMapper.ICustomQueryParameter ToDataTableParameter<TObject>(this IEnumerable<TObject> source,
			string typeName)
		{
			return source.ToDataTable().AsTableValuedParameter(typeName);
		}

		public static DataTable ToDataTable2<TObject>(this IEnumerable<TObject> source)
		{
			var result = new DataTable();

			var properties = typeof(TObject).GetProperties();


			foreach (var item in source)
			{
				foreach (var propertyInfo in properties)
				{
					var value = propertyInfo.GetValue(item);
					result.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
					result.Rows.Add(value);
				}
			}

			return result;
		}

		/// <summary>
		/// Converts the sequence of values of TValue value type into DataTable with single column.
		/// </summary>
		/// <typeparam name="TValue">The value type of values to convert.</typeparam>
		/// <param name="source">The source sequence of values.</param>
		/// <param name="columnName">The column name.</param>
		/// <returns>The converted DataTable with single column.</returns>
		public static DataTable ToDataTable<TValue>(this IEnumerable<TValue> source, string columnName)
			where TValue : struct
		{
			var result = new DataTable();

			result.Columns.Add(columnName, typeof(TValue));

			foreach (var item in source)
			{
				result.Rows.Add(item);
			}

			return result;
		}

		public static string EscapeLike(this string like)
		{
			return like.Replace("%", "!%")
				.Replace("_", "!_")
				.Replace("-", "!-")
				.Replace("[", "![")
				.Replace("]", "!]");
		}

		public static DataTable ToIdsTable(this IEnumerable<string> source)
		{
			var result = new DataTable("IdsList");

			result.Columns.Add("Id", typeof(string));

			foreach (var item in source)
			{
				result.Rows.Add(item);
			}

			return result;
		}

	}
}
