using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Models
{
    internal class SettingModel : ModelBase
    {
        public SettingModel() { }
        public SettingModel(SettingKeys key, string value, UserModel user)
        {
            Key = key;
            Value = value;
            User = user;
        }

        [Key]
        public int Id { get; set; }

        private UserModel _user;
        public UserModel User { get { return _user; } set => SetProperty(ref _user, value); }

        public SettingKeys Key { get; set; }

        private string _value = "";
        public string Value { get => _value; set => SetProperty(ref _value, value); }

        [NotMapped]
        public int ValueAsInt 
        {
            get
            {
                if (int.TryParse(Value, out int result))
                {
                    return result;
                }
                else
                {
                    throw new Exception($"Cant convert setting {Key} to int");
                }
            }
            set
            {
                Value = value.ToString();
            } 
        }

        public enum SettingKeys
        {
            GalleryPath,
            GalleryRowAmount,
            ThumbnailSize,
            SelectedOrder
        }
    }
}
