using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImageP.Annotations;

namespace ImageP
{
    class DatabaseManager
    {
        public DatabaseManager()
        {
            con = new SQLiteConnection("Data Source=database.sqlite");
            con.Open();
            new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Images (ID INTEGER PRIMARY KEY AUTOINCREMENT,Tags VARCHAR(45),ImagePath VARCHAR(45),unique(ImagePath))", con).ExecuteNonQuery();
            //ID,Tags,ImagePath
        }

        SQLiteConnection con;
        private static DatabaseManager instance;
        public static DatabaseManager getInstance()
        {
            if (instance == null){instance = new DatabaseManager();}
            return instance;
        }

        public void AddData(Image image)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO Images(Tags,ImagePath) VALUES(@tags,@imagepath)", con);
            command.Parameters.AddWithValue("tags", string.Join(",", image.Tags) );
            command.Parameters.AddWithValue("imagepath", image.Path);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception) { }
        }


        public List<Image> GetData(string query = null)
        {
            List<Image> list = new List<Image>();
            string q = "SELECT * FROM Images";
            if (!string.IsNullOrEmpty(query))
            {
                q += " WHERE Tags LIKE '%" + query + "%'";
            }
            SQLiteDataReader reader = new SQLiteCommand(q, con).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Image(reader));
            }
            return list;
        }
        public void DeleteData(Image image)
        {

            new SQLiteCommand(string.Format("DELETE FROM Images where ID={1}", image.ID), con).ExecuteNonQuery();
        }

        public bool UpdateData(Image image)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE Images SET Tags=@tags,ImagePath=@path WHERE ID=@ID", con);
            command.Parameters.AddWithValue("tags", string.Join(",", image.Tags));
            command.Parameters.AddWithValue("path", image.Path);
            command.Parameters.AddWithValue("ID", image.ID);
            int rows = command.ExecuteNonQuery();

            return rows > 0;
        }

    }

    public class Image:INotifyPropertyChanged
    {
        public Image(SQLiteDataReader reader)
        {
            ID = reader.GetInt32(reader.GetOrdinal("ID"));
            Path = reader.GetString(reader.GetOrdinal("ImagePath"));
            //Liste koyulup alınabilir yap 
            Tags = new ObservableCollection<string>(reader.GetString(reader.GetOrdinal("Tags")).Split(',').ToList());
            TagsStr = reader.GetString(reader.GetOrdinal("Tags"));
        }

        public string TagsStr{ get; set; }  
        public Image()
        {

        }
        public int ID { get; set; }


        private string _path;
        public string Path
        {
            get { return _path;}
            set { _path = value;OnPropertyChanged(); }
        }



        ObservableCollection<String> _tags;

        public ObservableCollection<String> Tags
        {
            get { return _tags ?? (_tags = new ObservableCollection<string>()); }
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
