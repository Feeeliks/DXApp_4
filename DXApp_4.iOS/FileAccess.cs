using DXApp_4.iOS;
using DXApp_4.Ressources;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileAccess))]
namespace DXApp_4.iOS
{
    public class FileAccess : IFileAccess
    {
        public string GetExternalStoragePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}