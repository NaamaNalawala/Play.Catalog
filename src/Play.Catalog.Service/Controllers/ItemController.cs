using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {

        private static readonly List<ItemDto> items = new List<ItemDto>(){
            new ItemDto(Guid.NewGuid(), "Potion", "Restores small amount of HP", 5, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 5, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 5, DateTimeOffset.Now),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).FirstOrDefault();
            if(item == null){
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.Now);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto){
            var existingItem = items.Where(item => item.Id == id).FirstOrDefault();
            if(existingItem == null){
                return NotFound();
            }
            var updatedItem = existingItem with {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var indexOfExistingItem = items.FindIndex(item => item.Id == id);
            items[indexOfExistingItem] = updatedItem;
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id){
            var index = items.FindIndex(item => item.Id == id);
            if(index < 0){
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent();
        }
    }
}