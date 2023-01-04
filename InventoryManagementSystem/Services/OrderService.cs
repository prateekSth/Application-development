using InventoryManagementSystem.Models;
using System.Text.Json;

namespace InventoryManagementSystem.Services;

public class OrderService
{
	// Save product data
	private static void Save_All(List<Order> orders)
	{
		var appDirectoryPath = Utils.Get_Directory_Path(); // adding directory path to store the data

		var app_Orders_FilePath = Utils.Get_Orders_FilePath();


		if (!Directory.Exists(appDirectoryPath))
		{
			Directory.CreateDirectory(appDirectoryPath);
		}
		//seralizing the order data
		var json = JsonSerializer.Serialize(orders);

		File.WriteAllText(app_Orders_FilePath, json);
	}

	//Method to get all the order list stored in the file
	public static List<Order> Get_All()
	{
		string appDirectoryPath = Utils.Get_Orders_FilePath();

		if (!File.Exists(appDirectoryPath))
		{
			return new List<Order>();
		}

		var json = File.ReadAllText(appDirectoryPath);

		var output = JsonSerializer.Deserialize<List<Order>>(json);

		return output;
	}
	//Order method to create a new order using userid, productid, quantitiy
	public static List<Order> Create(Guid userId, Guid productId, int quantity)
	{
		var order_day = (int)DateTime.Now.DayOfWeek + 1;

		var order_time = (int)DateTime.Now.Hour;

		//To gather order from monday to friday from 9 am to 6 pm
		//if (order_day >= 2 && order_day <= 6)
		//{
		//	if (order_time >= 9 && order_time <= 18)
		//	{
				var orders = Get_All();
				
				var products = ProductService.Get_All();

				var product = products.FirstOrDefault(x => x.Id == productId);

				//Exception when order quantity is more than stock quantity
				if (quantity > product.Quantity)
				{
					throw new Exception("Stock out. Sorry we have limited stock today.");
				}

				//Exception when order quantity is negative or less than 0
				if (quantity < 0 || quantity == 0)
				{
					throw new Exception("Please insert your order quantity");
				}
				//add order to the existing data 
				orders.Add(new Order
				{
					ProductId = productId,
					OrderedBy = userId,
					Quantity = quantity,
					OrderedDate = DateTime.Now,
				});

				Save_All(orders);

				return orders;
		//	}
		//	else
		//	{
		//		throw new Exception("Orders can be place between 9AM and 6PM only.");
		//	}
		//}
		//else
		//{
		//	throw new Exception("Orders can be placed between Monday and Friday only.");
		//}
		
	}

	public static List<Order> Update(Guid orderId, Guid userId)
	{

		var orders = Get_All();

		var products = ProductService.Get_All();

		var order = orders.FirstOrDefault(x => x.Id == orderId);

		var product = products.FirstOrDefault(x => x.Id == order.ProductId);

		if (order == null)
		{
			throw new Exception("Order not found.");
		}

		if(order.Quantity > product.Quantity)
		{
            throw new Exception("Cant order more than the existing quantity of the product.");
        }

        order.ApprovedBy = userId;
		order.IsApproved = true;
		order.ApprovedDate = DateTime.Now;
		product.Quantity -= order.Quantity;

		Save_All(orders);
		ProductService.Save_All(products);

		return orders;
	}
}
