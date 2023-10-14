using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;


namespace HelloFontExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.fontPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Roaming\hellofont\CoreSync\plugins\livetype\r";
            this.fontInfoTable = new DataTable();
            this.fontInfoTable.Columns.Add("序号", typeof(string));
            this.fontInfoTable.Columns.Add("名称", typeof(string));
            this.fontInfoTable.Columns.Add("类型", typeof(string));
            this.fontInfoTable.Columns.Add("路径", typeof(string));
        }

        private string fontPath;
        private DataTable fontInfoTable;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtHelp.AppendText("  1. 在『字由』客户端中激活需要的字体；\n  2. 点击『查询字体』按钮，获取字体名称和类型；\n  3. 点击『导出字体』按钮，程序自动将字体导出到『桌面\\字由字体』文件夹。");
        }

        private void btnSelectFont_Click(object sender, RoutedEventArgs e)
        {
            var fileNames = Directory.GetFiles(this.fontPath);
            this.fontInfoTable.Rows.Clear();
            foreach (string fileName in fileNames)
            {
                var fonts = new PrivateFontCollection();
                fonts.AddFontFile(fileName);

                var row = this.fontInfoTable.NewRow();
                row["序号"] = this.fontInfoTable.Rows.Count + 1;
                row["名称"] = fonts.Families[0].Name;
                row["类型"] = fileName.Substring(fileName.LastIndexOf('.')); ;
                row["路径"] = fileName;

                this.fontInfoTable.Rows.Add(row);
            }
            this.fontInfoGrid.ItemsSource = this.fontInfoTable.DefaultView;
        }

        private void btnExportFont_Click(object sender, RoutedEventArgs e)
        {
            string targetDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\字由字体";
            Directory.CreateDirectory(targetDirectory);
            foreach (DataRow row in this.fontInfoTable.Rows)
            {
                string sourcePath = (string)row["路径"];
                var targetPath = $@"{targetDirectory}\{(string)row["名称"]}{(string)row["类型"]}";
                File.Copy(sourcePath, targetPath, true);
            }
        }
    }
}
