using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using packt_asp_net_core_and_angular2.ViewModels;
using Newtonsoft.Json;
using packt_asp_net_core_and_angular2.Data;
using AutoMapper;
using packt_asp_net_core_and_angular2.Data.Items;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace packt_asp_net_core_and_angular2.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private int DefaultNumberOfItems = 5;

        private int MaxNumberOfItems = 100;

        private ApplicationDbContext _dbContext;

        private IMapper _mapper;

        public ItemsController(ApplicationDbContext context, IMapper mapper)
        {
            this._dbContext = context;
            this._mapper = mapper;
        }

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
            var item = this._dbContext.Items.Find(id);
            if (item == null) return NotFound(new {
                Error = $"Item ID {id} has not been found."
            });
            return new JsonResult(this._mapper.Map<ItemViewModel>(item), DefaultJonSerializerSetting);
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
            var arr = this._dbContext.Items
                .OrderByDescending(i => i.CreatedDate)
                .Take(num).ToArray();
            return new JsonResult(arr.Select(this._mapper.Map<ItemViewModel>), DefaultJonSerializerSetting);
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
            var arr = this._dbContext.Items
                .OrderByDescending(i => i.ViewCount)
                .Take(n).ToArray().Select(this._mapper.Map<ItemViewModel>);
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
            var arr = this._dbContext.Items
                .OrderBy(i => Guid.NewGuid())
                .Take(n).ToArray().Select(this._mapper.Map<ItemViewModel>);
            return new JsonResult(arr, DefaultJonSerializerSetting);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add([FromBody] ItemViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            var item = this._mapper.Map<Item>(model);
            item.CreatedDate =
            item.LastModifiedDate = DateTime.Now;

            item.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this._dbContext.Items.Add(item);
            this._dbContext.SaveChanges();

            return new JsonResult(this._mapper.Map<ItemViewModel>(item), this.DefaultJonSerializerSetting);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Edit(int id, [FromBody] ItemViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            var item = this._dbContext.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            item.UserId = model.UserId;
            item.Description = model.Description;
            item.Flags = model.Flags;
            item.Notes = model.Notes;
            item.Text = model.Text;
            item.Title = model.Title;
            item.Type = model.Type;
            item.LastModifiedDate = DateTime.Now;

            this._dbContext.SaveChanges();

            return new JsonResult(this._mapper.Map<ItemViewModel>(item), this.DefaultJonSerializerSetting);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var item = this._dbContext.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            this._dbContext.Items.Remove(item);
            this._dbContext.SaveChanges();
            return Ok();
        }
    }
}
