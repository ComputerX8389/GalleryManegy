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

        // Todo Get from settings
        private readonly List<string> AllowedExtensions = new()
        {
            "PNG",
            "JPEG",
            "JPG"
        };
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
            while (true)
            {
                Debug.WriteLine("lol");
                    await Task.Delay(2000);
            }
        }

        private async Task ScanFolder(DirectoryInfo path)
        {       
            var files = path.GetFiles();
            var dirs = path.GetDirectories();

            foreach (var file in files)
            {
                if (AllowdExtension(file))
                {
                    AddImage(file);
                }
                else
                {
                    AddUnsurported(file);
                }
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
                Debug.WriteLine($"Image {file.Name} already in database");
            }
        }

        private void AddUnsurported(FileInfo file)
        {
            var unsupported = Dbcontext.UnsupportedFiles.FirstOrDefault(i => i.FullName == file.FullName);

            if (unsupported == null)
            {
                Debug.WriteLine($"Adding UnsupportedFile {file.Name}");
                Dbcontext.UnsupportedFiles.Add(new UnsupportedFile(file, User));
            }
            else
            {
                Debug.WriteLine($"UnsupportedFile {file.Name} already in database");
            }
        }

        private bool AllowdExtension(FileInfo file)
        {
            foreach (var extension in AllowedExtensions)
            {
                if (file.Extension.Remove(0, 1).ToLower() == extension.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
