using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GalleryManegy.Models;

namespace GalleryManegy.Handlers
{
    internal class FileScanner
    {
        private readonly DatabaseHandler DatabaseHandler;

        private bool Scanning;

        public FileScanner(DatabaseHandler databaseHandler)
        {
            DatabaseHandler = databaseHandler;
        }

        public async Task ScanAsync()
        {
            var path = DatabaseHandler.GetSetting(SettingModel.SettingKeys.GalleryPath);
            var directory = new DirectoryInfo(path.Value);
            // Dont scan if already scanning
            if (Scanning == false)
            {
                Scanning = true;
                await ScanFolder(directory);
                ScanForDeleted(directory);
                Scanning = false;
            }
            else
            {
                Debug.WriteLine("Scan already running");
            }
        }

        private async Task ScanFolder(DirectoryInfo path)
        {
            var files = path.GetFiles();
            var dirs = path.GetDirectories();

            foreach (var file in files)
            {
                AddImage(file);
                // Do not strees user computer too much. Wait a bit before next picture
                // Todo Get from settings
                await Task.Delay(100);
            }

            // Bulk images together for better performance
            DatabaseHandler.SaveChanges();

            foreach (var dir in dirs)
            {
                await ScanFolder(dir);
            }
        }

        private void ScanForDeleted(DirectoryInfo path)
        {
            var imagesToDelete = new List<ImageModel>();
            var images = DatabaseHandler.GetAllImages();

            foreach (var image in images)
            {
                // Remove file from db if root path has chaged
                if (!image.FullName.Contains(path.FullName))
                {
                    imagesToDelete.Add(image);
                }

                // Remove file from db if removed from disk
                if (!File.Exists(image.FullName))
                {
                    imagesToDelete.Add(image);
                }
            }

            Debug.WriteLine($"Removing {imagesToDelete.Count} images");

            DatabaseHandler.RemoveRange(imagesToDelete);
            DatabaseHandler.SaveChanges();
        }

        private void AddImage(FileInfo file)
        {
            var img = DatabaseHandler.GetImageByFullName(file.FullName);

            if (img == null)
            {
                Debug.WriteLine($"Adding image {file.Name}");
                DatabaseHandler.AddImage(new ImageModel(file));
            }
            else
            {
                Debug.WriteLine($"Updating image {file.Name}");
                img.Update(file);
            }
        }
    }
}
