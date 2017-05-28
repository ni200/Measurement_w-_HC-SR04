using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace SiloMeasurement
{
    /// <summary>
    /// SQLite data object, 
    /// describes a row of the table "SiloData" in the database "Silo.sqlite"
    /// and can communicate with the database.
    /// </summary>
    public class SiloData
    {
        private static SQLiteConnection connection;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        // silo fill level in percentage
        [NotNull]
        public double Value { get; set; }

        // time the value was saved
        [NotNull]
        public string TimeString { get; set; }

        public DateTime Time
        {
            get { return Convert.ToDateTime(TimeString); }
        }

        static SiloData()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Silo.sqlite");

            connection = new SQLiteConnection(
                            new SQLitePlatformWinRT(), path);

            connection.CreateTable<SiloData>();
        }

        public static SiloData Read(DateTimeOffset id)
        {
            return connection.Find<SiloData>(id);
        }

        public static List<SiloData> Read(string query)
        {
            List<SiloData> result;
            try { result = connection.Query<SiloData>(query); } catch { result = new List<SiloData>(); }
            return result;
        }

        public static List<SiloData> ReadAll()
        {
            var table = connection.Table<SiloData>();
            List<SiloData> result = new List<SiloData>();
            foreach (SiloData item in table)
            {
                result.Add(item);
            }
            return result;
        }

        public static bool CloseConnection()
        {
            bool result = false;
            try
            {
                connection.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public int Insert()
        {
            return connection.Insert(this);
        }
    }
}
