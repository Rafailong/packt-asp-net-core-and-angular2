using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using packt_asp_net_core_and_angular2.ViewModels;
using Newtonsoft.Json;

namespace packt_asp_net_core_and_angular2.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private int DefaultNumberOfItems = 5;

        private int MaxNumberOfItems = 100;

        public JsonSerializerSettings DefaultJonSerializerSetting {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };
            } 
        }

        [HttpGet]
        public IActionResult Get()
        {
            return NotFound(new {
                Error = "not found"
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = GetSampleItem()
                .Where(i => i.Id == id)
                .FirstOrDefault();
            return new JsonResult(item, DefaultJonSerializerSetting);
        }

        // GET api/values
        [HttpGet("GetLatest")]
        public IActionResult GetLatest()
        {
            return GetLatest(DefaultNumberOfItems);
        }

        [HttpGet("GetLatest/{num}")]
        public IActionResult GetLatest(int num)
        {
            num = num > MaxNumberOfItems ? DefaultNumberOfItems : num;
            var arr = GetSampleItem(num)
                .OrderByDescending(i => i.CreatedDate)
                .Take(num);
            return new JsonResult(arr, DefaultJonSerializerSetting);
        }

        [HttpGet("GetMostViewed")]
        public IActionResult GetMostViewed()
        {
            return GetMostViewed(DefaultNumberOfItems);
        }

        [HttpGet("GetMostViewed/{n}")]
        public IActionResult GetMostViewed(int n)
        {
            n = n > MaxNumberOfItems ? DefaultNumberOfItems : n;
            var arr = GetSampleItem(n)
                .OrderByDescending(i => i.ViewCount)
                .Take(n);
            return new JsonResult(arr, DefaultJonSerializerSetting);
        }

        [HttpGet("GetRandom")]
        public IActionResult GetRandom()
        {
            return GetRandom(DefaultNumberOfItems);
        }

        [HttpGet("GetRandom/{n}")]
        public IActionResult GetRandom(int n)
        {
            n = n > MaxNumberOfItems ? DefaultNumberOfItems : n;
            var arr = GetSampleItem(n)
                .OrderBy(i => Guid.NewGuid())
                .Take(n);
            return new JsonResult(arr, DefaultJonSerializerSetting);
        }

        private IList<ItemViewModel> GetSampleItem(int n = 999)
        {
            var arr = new List<ItemViewModel>();
            var date = new DateTime(2016, 12, 31).AddDays(-n);
            for (int i = 0; i < n; i++)
            {
                arr.Add(new ItemViewModel
                {
                    Id = i,
                    Title = $"Item {i} Title",
                    Description = $"This is a sample description for {i}. Lorem ipsum dolor sit amet.",
                    CreatedDate = date.AddDays(i),
                    LastModifiedDate = date.AddDays(i),
                    ViewCount = n - i
                });
            }
            
            return arr;
        }
    }
}
