using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GalleryManegy.Handlers.DatabaseHandler;

namespace GalleryManegy.Models
{
    internal class ComboDataModel
    {
        public ComboDataModel(SortingOptions id, string value)
        {
            SortingOption = id;
            HumanText = value;
        }
        public SortingOptions SortingOption { get; set; }
        public string HumanText { get; set; }

        public override string ToString()
        {
            return HumanText;
        }
    }
}
