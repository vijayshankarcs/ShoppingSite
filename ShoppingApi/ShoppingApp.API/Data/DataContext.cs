﻿using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.Models;
namespace ShoppingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Value> Values { get; set; }
    }
}