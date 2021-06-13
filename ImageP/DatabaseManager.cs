using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ImageP
{
    public class DatabaseManager
    {
        private static DatabaseManager instance;

        private readonly SQLiteConnection con;

        public DatabaseManager()
        {
            con = new SQLiteConnection("Data Source=database.sqlite");
            con.Open();
            new SQLiteCommand(
                @"CREATE TABLE IF NOT EXISTS Images (ID INTEGER PRIMARY KEY AUTOINCREMENT,Tags VARCHAR(45),ImagePath VARCHAR(45),unique(ImagePath))",
                con).ExecuteNonQuery();
            //ID,Tags,ImagePath
        }

        public static DatabaseManager getInstance()
        {
            if (instance == null) instance = new DatabaseManager();
            return instance;
        }

        public void AddData(Image image)
        {
            var command = new SQLiteCommand("INSERT INTO Images(Tags,ImagePath) VALUES(@tags,@imagepath)", con);
            command.Parameters.AddWithValue("tags", string.Join(",", image.Tags));
            command.Parameters.AddWithValue("imagepath", image.Path);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
        }


        public List<Image> GetData(string query = null)
        {
            var list = new List<Image>();
            var q = "SELECT * FROM Images";
            if (!string.IsNullOrEmpty(query)) q += " WHERE Tags LIKE '%" + query + "%'";
            var reader = new SQLiteCommand(q, con).ExecuteReader();
            while (reader.Read()) list.Add(new Image(reader));
            return list;
        }

        public void DeleteData(Image image)
        {
            new SQLiteCommand(string.Format("DELETE FROM Images where ID={1}", image.ID), con).ExecuteNonQuery();
        }

        public bool UpdateData(Image image)
        {
            var command = new SQLiteCommand("UPDATE Images SET Tags=@tags,ImagePath=@path WHERE ID=@ID", con);
            command.Parameters.AddWithValue("tags", string.Join(",", image.Tags));
            command.Parameters.AddWithValue("path", image.Path);
            command.Parameters.AddWithValue("ID", image.ID);
            var rows = command.ExecuteNonQuery();

            return rows > 0;
        }
    }
}