using Distributed.Cache.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distributed.Cache.Context
{
    public class OrderContext
    {
        public async Task<List<Order>> GetOrders()
        {
            //Simulating a database call;
            await Task.Delay(1500);

            return new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    CustomerId = "1234",
                    EmployeeId = 1,
                    RequiredDate = DateTime.Now,
                    ShippedDate = DateTime.Now.AddDays(5),
                    ShipCity = "Recife",
                    ShipName = "Amazon"
                },
                new Order()
                {
                    OrderId = 2,
                    CustomerId = "4321",
                    EmployeeId = 2,
                    RequiredDate = DateTime.Now,
                    ShippedDate = DateTime.Now.AddDays(3),
                    ShipCity = "Goiania",
                    ShipName = "Shopee"
                }
            };
        }
    }
}
