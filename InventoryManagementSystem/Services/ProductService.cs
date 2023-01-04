using InventoryManagementSystem.Models;
using System.Text.Json;

namespace InventoryManagementSystem.Services;

public class ProductService
{
	public static void Save_All(List<Product> products)// method to save product list
	{
		var appDirectoryPath = Utils.Get_Directory_Path(); // directory path to store list in json format
		var app_Products_FilePath = Utils.Get_Products_FilePath();

		if (!Directory.Exists(appDirectoryPath))
		{
			Directory.CreateDirectory(appDirectoryPath);
		}

		var json = JsonSerializer.Serialize(products); 

		File.WriteAllText(app_Products_FilePath, json); //adding new list to existing data
	}

	public static List<Product> Get_All() //method to fetch exisitng list from product file
	{
		string appDirectoryPath = Utils.Get_Products_FilePath();

		if (!File.Exists(appDirectoryPath))
		{
			return new List<Product>();
		}

		var json = File.ReadAllText(appDirectoryPath);

		var output = JsonSerializer.Deserialize<List<Product>>(json);

		return output;
	}

	public static Product Get_By_Id(Guid id) //fetch data by id from json file
	{
		var products = Get_All();
		return products.FirstOrDefault(x => x.Id == id);
	}

	public static List<Product> Create(Guid userId, string title, int quantity) // method to create new product 
	{
		if (quantity <= 0)
		{
			throw new Exception("Please insert positive value.");
		}

		var products = Get_All();
		
		var product = new Product()
		{
            Title = title,
            Quantity = quantity,
            CreatedBy = userId
        };

		products.Add(product);

		Save_All(products);

		return products;
	}

	public static List<Product> Update(Guid productId, string title, int quantity) //method to update existing product 
	{
		var products = Get_All();

		var product = products.FirstOrDefault(x => x.Id == productId);

		if (product == null)
		{
			throw new Exception("Product not available.");
		}

		product.Title = title;
		product.Quantity = quantity;
		product.LastModifiedAt = DateTime.Now;

		Save_All(products);

		return products;
	}

	public static List<Product> Delete(Guid id) //method to delete product 
	{
		var products = Get_All();

		var product = products.FirstOrDefault(x => x.Id == id);

		if (product == null)
		{
			throw new Exception("Product not found.");
		}

		products.Remove(product);

		Save_All(products);
		
		return products; //return product after removing of product
	}
}
