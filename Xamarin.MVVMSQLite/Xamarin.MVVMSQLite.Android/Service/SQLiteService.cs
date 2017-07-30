using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using Xamarin.MVVMSQLite.Interfaces;
using Environment = System.Environment;


[assembly: Dependency(typeof(Xamarin.MVVMSQLite.Droid.SQLiteService))]
namespace Xamarin.MVVMSQLite.Droid
{
    public class SQLiteService : ISQLiteService
    {
        string GetPath(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw  new ArgumentException("Banco de dados Inválido!", nameof(databaseName));
            }

            var sqlFileName = $"{databaseName}.db3";
            var documentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqlFileName);

            return path;
        }

        public SQLiteConnection GetConnection(string databaseName)
        {
            return new SQLiteConnection(new SQLitePlatformAndroid() , GetPath(databaseName));
        }

        public long GetSize(string databaseName)
        {
            var fileInfo =  new FileInfo(databaseName);
            return fileInfo != null ? fileInfo.Length : 0;
        }
    }
}