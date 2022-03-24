using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Handlers
{
    internal class FileScanner
    {
        private readonly DatabaseContext dbcontext;

        public FileScanner(DatabaseContext context)
        {
            dbcontext = context;

            Task.Run(() => ScanAsync());
        }

        private async Task ScanAsync()
        {
            while (true)
            {
                Debug.WriteLine("lol");
                await Task.Delay(2000);
            }
        }
    }
}
