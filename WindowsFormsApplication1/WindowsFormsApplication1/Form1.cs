using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //抓取key
            var VisionApiKey = textBoxkey.Text.Trim();
            //讀取圖檔
            var openDlg = new OpenFileDialog();
            //圖檔過濾類型
            openDlg.Filter = "JPEG Image(*.jpg)|*.jpg|*.png|*.png";
            //沒選檔案
            if (openDlg.ShowDialog(this) != DialogResult.OK) return;
  
            //取得選擇的檔案名稱
            string filePath = openDlg.FileName;

            //顯示原始圖片
            var FileStream = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
            pictureBox1.Image = System.Drawing.Image.FromStream(FileStream);
            FileStream.Close();

            //OCR OcrResults
            OcrResults OcrResults;
            //建立VisionServiceClient
            var visionClient = new Microsoft.ProjectOxford.Vision.VisionServiceClient(VisionApiKey);

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                this.textBox.Text = "辨識中...";
                //以繁體中文辨識
                OcrResults = visionClient.RecognizeTextAsync(fs, LanguageCodes.AutoDetect).Result;
                this.textBox.Text = "";
            }

            this.textBox.Text = "";
            //抓取每一區塊的辨識結果
            foreach (var Region in OcrResults.Regions)
            {
                //抓取每一行
                foreach (var line in Region.Lines)
                {
                    //抓取每一個字
                    foreach (var Word in line.Words)
                    {
                        //顯示辨識結果
                        this.textBox.Text += Word.Text;
                    }
                    //加換行
                    this.textBox.Text += "\n";
                }
            }
        }
    }
}
