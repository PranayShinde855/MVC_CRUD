using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVC_CRUD.Models;

namespace MVC_CRUD.Services
{
    public class DBModel : DbContext
    {
        public DbSet<Product> products { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Role> roles { get; set; }
    }
}