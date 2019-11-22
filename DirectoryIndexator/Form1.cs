using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryIndexator
{
    public partial class Form1 : Form
    {
        //static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        //static int alarmCounter = 1;
        //static bool exitFlag = false;
        private int cFiles = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(@"C:\Program Files");

            FileSystemIndexer.IndexDirectory(directory);

            timer1.Enabled = true;

            //ThreadPool.QueueUserWorkItem(work =>
            //{
            //    while (timer1.Enabled)
            //    {
            //        cFiles = FileSystemIndexer.Index.Count;
            //    }
            //});        
            

        }



        private void button4_Click(object sender, EventArgs e)
        {
             var total = FileSystemIndexer.Index.Select(p => p.Value.Count).Sum();

            if (FileSystemIndexer.Index.ContainsKey(textBox2.Text))
            {
                var value = FileSystemIndexer.Index[textBox2.Text];
                string valueDisplay = string.Empty;
                value.ToList().ForEach(p => valueDisplay += (p + Environment.NewLine));

                MessageBox.Show(valueDisplay);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text ="Файлов: " + FileSystemIndexer.cFiles + "  Папок: " + FileSystemIndexer.cFolders;
            if (cFiles < FileSystemIndexer.cFiles)
            {
                cFiles = FileSystemIndexer.cFiles;                
            }             
            else
            if (cFiles == FileSystemIndexer.cFiles)
            {
                timer1.Enabled = false;
                MessageBox.Show("индексирование файлов завершено");                
            }
        }
    }
}
