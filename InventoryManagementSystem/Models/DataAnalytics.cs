using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public static class DataAnalytis
    {
        public static List<DataAnalytisDTO> DataAnalytisDTO(Guid userId)
        {
            List<DataAnalysisDTO> dataAnalysisDTO = new List<DataAnalysisDTO>();
            var data = ProductService.Get_All_Users();
            //var filterData = data.Where(x => x.Id == userId).ToList();
            foreach (var item in data)
            {
                dataAnalysisDTO.Add(new DataAnalysisDTO { TitleName = item.ItemName, ValueCount = item.Quantity });
            }
            return dataAnalytisDTO;
        }
    }

    public class DataAnalytisDTO
    {
        public string TitleName { get; set; }
        public int ValueCount { get; set; }
    }
}
