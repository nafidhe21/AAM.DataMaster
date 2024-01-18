using AAM.DataMaster.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AAM.DataMaster.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext db;

        public ProductController(AppDbContext appDbContext) =>
            db = appDbContext;

        [HttpGet("product")]
        public async Task<IActionResult> Get()
        {
            var productList = await db.Products.ToListAsync();
            return Ok(new { productList });
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Stock = dto.Stock,
                    Price = dto.Price,
                };
                db.Products.Add(product);
                await db.SaveChangesAsync();

                return Ok(new { message = "Created", product });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("process-transaction")]
        public async Task<IActionResult> ProcessTransactionRequest([FromBody] TransactionRequestDto dto)
        {
            try
            {
                var product = await db.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId);

                if(product.Stock == 0)
                {
                    return NotFound("Item Stock empty");
                };

                product.Stock -= dto.Amount;
                int price = product.Price * dto.Amount;

                await db.SaveChangesAsync();

                var transactionResponse = new TransactionRequestResponseDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = price.ToString(),
                    Amount = dto.Amount,
                };

                return Ok(transactionResponse);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
