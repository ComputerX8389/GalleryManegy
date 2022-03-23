using GalleryManegy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<UserModel> Users { get; set; }


    }
}
