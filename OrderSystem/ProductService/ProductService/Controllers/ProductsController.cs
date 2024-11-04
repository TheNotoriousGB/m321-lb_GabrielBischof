using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using System.Collections.Generic;
using RabbitMQ.Client;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;    

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var message = $"New product added: {product.Name} with price {product.Price}";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "product_updates", basicProperties: null, body: body);

            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
        }
    }
}


