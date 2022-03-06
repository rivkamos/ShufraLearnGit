using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FMRCommon;
using FMRCore.FMRService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
namespace ProjApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StocksController : ControllerBase
	{
		private readonly ILogger<StocksController> _logger;
		private readonly IDataService _service;
		private readonly IMemoryCache _memoryCache;
		private readonly string cacheKey = "stocksData";
		private MemoryCacheEntryOptions cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddMinutes(50),
			Priority = CacheItemPriority.High,
			SlidingExpiration = TimeSpan.FromSeconds(20)
		};
		public StocksController(ILogger<StocksController> logger, IDataService services, IMemoryCache memoryCache)
		{
			_logger = logger;
			_service = services;
			_memoryCache = memoryCache;
		}

		[HttpGet("Search/{term}")]
		public List<Stock> Search(string term)
		{
			try
			{
				List<Stock>  data = GetStocksData();
				return data.Where(w => w.Classification.Contains(term) || w.LongName.Contains(term)).ToList();

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		[HttpGet("{id}")]
		public Stock GetStockById(string id)
		{
			try
			{
				List<Stock> data = GetStocksData();
				return data.FirstOrDefault(w => w.Id == id);
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		[HttpGet("Search")]
		public List<Stock> Search([FromBody]  List<double> range)
		{
			try
			{
				List<Stock> data = GetStocksData();
				return data.Where(w => range[0] >= w.SellPriceExp && range[1] <= w.SellPriceExp).ToList();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private List<Stock> GetStocksData() {
			if (!_memoryCache.TryGetValue(cacheKey, out List<Stock> data))
			{
				data = _service.GetStocks();
				_memoryCache.Set(cacheKey, data, cacheExpiryOptions);
			}
			return data;
		}
	}
}
