using Microsoft.AspNetCore.Mvc;
using Play.Common.Service.Repositories;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemRepository;
        public ItemsController(IRepository<InventoryItem> itemRepository)
        {
            this.itemRepository = itemRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }
            var items = (await itemRepository.GetAllAsync(item =>item.UserId == userId))
            .Select(item => item.AsDto());
            return Ok(items);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await itemRepository.GetAsync(
                item => item.UserId == grantItemsDto.UserId 
                && item.CatalogItemId == grantItemsDto.CatalogItemId);
            
            if(inventoryItem == null){
                inventoryItem = new InventoryItem{
                    UserId = grantItemsDto.UserId,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await itemRepository.CreateAsync(inventoryItem);
            }
            else{
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemRepository.UpdateAsync(inventoryItem);
            }
            return Ok();

        }
    }
}