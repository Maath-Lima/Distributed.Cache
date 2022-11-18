using Distributed.Cache.Context;
using Distributed.Cache.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Distributed.Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly OrderContext _orderContext;

        public OrderController(
            IDistributedCache distributedCache,
            OrderContext orderContext)
        {
            _distributedCache = distributedCache;
            _orderContext = orderContext;
        }

        [HttpGet("redis")]
        [ProducesResponseType(typeof(IList<Order>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrdersWithRedisCache()
        {
            var cacheKey = "orderList";
            string serializedOrderList = string.Empty;
            var orderList = new List<Order>();

            var redisOrderList = await _distributedCache.GetAsync(cacheKey);

            if (redisOrderList != null)
            {
                serializedOrderList = Encoding.UTF8.GetString(redisOrderList);

                orderList = JsonConvert.DeserializeObject<List<Order>>(serializedOrderList);
            }
            else
            {
                orderList = await _orderContext.GetOrders();

                serializedOrderList = JsonConvert.SerializeObject(orderList);

                redisOrderList = Encoding.UTF8.GetBytes(serializedOrderList);

                var redisOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisOrderList, redisOptions);
            }

            return Ok(orderList);
        }
    }
}
