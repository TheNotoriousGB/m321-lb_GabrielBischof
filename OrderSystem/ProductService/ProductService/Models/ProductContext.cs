﻿using Microsoft.EntityFrameworkCore;
using ProductService.Models;

public class ProductsContext : DbContext
{
    public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}
