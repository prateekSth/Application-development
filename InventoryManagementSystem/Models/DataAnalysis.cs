using InventoryManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public static class DataAnalysis
    {
        public static List<DataAnalysisDTO> DataAnalysisDTO()
        {
            List<DataAnalysisDTO> dataAnalysisDTO = new List<DataAnalysisDTO>();
            var data = ProductService.Get_All();
            foreach (var item in data)
            {
                dataAnalysisDTO.Add(new DataAnalysisDTO { TitleName = item.Title, ValueCount = item.Quantity });
            }
            return dataAnalysisDTO;
        }
    }
    public class DataAnalysisDTO
    {
        public string TitleName { get; set; }
        public int ValueCount { get; set; }
    }
}
