using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace playground_serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonContents = File.ReadAllText("todeserialize.json");

            var productSalesDto = JsonConvert.DeserializeObject<ProductSalesDto>(jsonContents);

            var dailySupplierSales = productSalesDto.SupplierSales.SelectMany(ss => ss.DailySales).GroupBy(ds => ds.Day)
                .Select(grouping =>
                    new SalesPerDay {Day = grouping.Key, Sales = grouping.Sum(group => group.Quantity)}).OrderByDescending(salesPerDay => salesPerDay.Day).ToList();

            foreach (var dailySupplierSale in dailySupplierSales)
            {
                Console.WriteLine($"{nameof(SalesPerDay)}: {dailySupplierSale.Day}: {dailySupplierSale.Sales}");
            }
        }
    }

    public class SalesPerDay
    {
        public DateTime Day { get; set; }
        public int Sales { get; set; }
    }

    public class ProductSalesDto
    {
        public ProductDto Product { get; set; }
        public IEnumerable<SupplierSalesDto> SupplierSales { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SupplierSalesDto
    {
        public SupplierDto Supplier { get; set; }
        public IEnumerable<DailySupplierSalesDto> DailySales { get; set; }
    }
    
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class DailySupplierSalesDto
    {
        public DateTime Day { get; set; }
        public int Quantity { get; set; }
    }
}