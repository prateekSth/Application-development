using InventoryManagementSystem.Models;
using System.Text.Json;

namespace InventoryManagementSystem.Services;

public class StatsService
{
    public static List<Order> Get_Approved_Orders()
    {
        string appDirectoryPath = Utils.Get_Orders_FilePath();

        if (!File.Exists(appDirectoryPath))
        {
            return new List<Order>();
        }

        var json = File.ReadAllText(appDirectoryPath);

        var result = JsonSerializer.Deserialize<List<Order>>(json);

        var orders = result.Where(x => x.IsApproved).ToList();

        return orders;
    }

    public static List<OrderQuantity> Get_Ordered_Quantity()
    {
        var orders = Get_Approved_Orders();

        var output = (from order in orders
                     group order by order.ProductId
                     into item
                     select new OrderQuantity
                     {
                         Item = item.Key,
                         Quantity = item.Sum(x => x.Quantity)
                     }).ToList();

        return output;
    }
}
