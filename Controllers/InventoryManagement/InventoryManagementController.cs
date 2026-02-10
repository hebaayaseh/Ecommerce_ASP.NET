using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_ASP.NET.Controllers.InventoryManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryManagementController : ControllerBase
    {
        public readonly InventoryManagementManager inventoryManagement;
        public InventoryManagementController(InventoryManagementManager inventoryManagement)
        {
            this.inventoryManagement = inventoryManagement;
        }

    }
}
