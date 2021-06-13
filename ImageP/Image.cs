using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using ImageP.Annotations;

namespace ImageP
{
    public class Image : INotifyPropertyChanged
    {
        private string _path;


        private ObservableCollection<string> _tags;

        public Image(SQLiteDataReader reader)
        {
            ID = reader.GetInt32(reader.GetOrdinal("ID"));
            Path = reader.GetString(reader.GetOrdinal("ImagePath"));
            Tags = new ObservableCollection<string>(reader.GetString(reader.GetOrdinal("Tags")).Split(',').ToList());
            TagsStr = reader.GetString(reader.GetOrdinal("Tags"));
        }

        public Image()
        {
        }

        public string TagsStr { get; set; }
        public int ID { get; set; }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Tags
        {
            get => _tags ?? (_tags = new ObservableCollection<string>());
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}