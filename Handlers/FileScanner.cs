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
        private readonly DatabaseContext Dbcontext;
        private readonly UserModel User;

        private readonly DirectoryInfo StartingDir = new(@"C:\Users\STEFF\Desktop\Pics");

        public FileScanner(DatabaseContext context, UserModel currentUser)
        {
            Dbcontext = context;
            User = currentUser;

            Task.Run(() => ScanAsync());
        }

        private async Task ScanAsync()
        {
            await ScanFolder(StartingDir);
            ScanForDeleted(StartingDir);
            while (true)
            {
                Debug.WriteLine("lol");
                await Task.Delay(2000);
            }
        }

        private async Task ScanFolder(DirectoryInfo path)
        {
            await Task.Delay(5000);
            var files = path.GetFiles();
            var dirs = path.GetDirectories();

            foreach (var file in files)
            {
                AddImage(file);
                // Do not strees user computer too much. Wait a bit before next pixture
                // Todo Get from settings
                await Task.Delay(100);
            }

            Dbcontext.SaveChanges();

            foreach (var dir in dirs)
            {
                await ScanFolder(dir);
            }
        }

        private void ScanForDeleted(DirectoryInfo path)
        {
            var imagesToDelete = new List<ImageModel>();
            var images = Dbcontext.Images.Where(i => i.User.Id == User.Id);

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

            Dbcontext.Images.RemoveRange(imagesToDelete);
            Dbcontext.SaveChanges();
        }

        private void AddImage(FileInfo file)
        {
            var img = Dbcontext.Images.FirstOrDefault(i => i.FullName == file.FullName);

            if (img == null)
            {
                Debug.WriteLine($"Adding image {file.Name}");
                Dbcontext.Images.Add(new ImageModel(file, User));
            }
            else
            {
                Debug.WriteLine($"Updating image {file.Name}");
                img.Update(file, User);
            }
        }
    }
}
