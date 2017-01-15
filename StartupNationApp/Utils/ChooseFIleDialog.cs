using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupNationApp.Utils
{
    public class ChooseFIleDialog
    {
        public static string GetFilePath(bool checkFileExists)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.IO.Path.GetFullPath(Environment.SpecialFolder.MyDocuments.ToString());
            dialog.Multiselect = false;
            dialog.CheckFileExists = checkFileExists;
            var result = dialog.ShowDialog();
            if ((bool)result)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }
    }
}
