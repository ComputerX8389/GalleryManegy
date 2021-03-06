using GalleryManegy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<SettingModel> Settings { get; set; }

        public DatabaseContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GalleryManegy;User Id=sa;password=GUwNLNnmvD3Dn5DqprBs");
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var filepath = Path.Combine(path, "GalleryManegy", "database.sql");
            //optionsBuilder.UseSqlite("Data Source=GalleryManegy.db");
        }
    }
}
