using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace xml解析
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sPath = "";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择所有文件存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {

                sPath = folder.SelectedPath;
                //MessageBox.Show(sPath);
            }

            //string[] files = GetOpenFileDialogReturnFileFullName();
            string[] files = Directory.GetFiles(sPath, "*.xml", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                this.textBox1.AppendText(files[i] + "\r\n");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                XmlToDataTableByFile();
            }
        }

        // Xml结构的文件读到DataTable中
        private void XmlToDataTableByFile()
        {
            string fileNames = textBox1.Text;
            string[] files = fileNames.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i];
                XmlDocument doc = new XmlDocument();
                if (fileName != "")
                {
                    doc.Load(fileName);

                    DataSet ds = new DataSet();
                    TextReader tr = new StringReader(doc.InnerXml);
                    ds.ReadXml(tr);

                    DataTable dt = ds.Tables[0];
                    dt.Columns["ModelName"].SetOrdinal(4);

                    DataTableToString(dt, fileName);
                }
            }
            MessageBox.Show("数据导出", "转换成功!");
        }

        public static string DataTableToString(DataTable dt, string filePath)
        {
            filePath = filePath.Replace("xml", "txt");
            System.Text.StringBuilder builder = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                builder.AppendLine(dr[0].ToString() + '&' + dr[1].ToString() + '&' + dr[2].ToString() + '&' + dr[3].ToString() + '&' + dr[4].ToString());
                //builder.AppendLine('('+dr[0].ToString() + ',' + dr[1].ToString() + ")&" + dr[2].ToString() + '&' + dr[3].ToString() + '&' + dr[4].ToString());
            }
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                UTF8Encoding utf8 = new UTF8Encoding(false);
                using (System.IO.TextWriter text = new System.IO.StreamWriter(file, utf8))
                {
                    text.Write(builder.ToString());
                }
            }
            return "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sPath = this.textBox4.Text;
            string[] files = Directory.GetFiles(sPath, "*.xml", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                this.textBox2.AppendText(files[i] + "\r\n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sPath = this.textBox5.Text;

            string[] files = Directory.GetFiles(sPath, "*.xml", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                this.textBox3.AppendText(files[i] + "\r\n");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text != "" && this.textBox3.Text != "")
            {
                getXMLData();
            }
        }

        private void getXMLData()
        {
            //转坐标的xml
            string fileNames1 = textBox2.Text;
            string[] files1 = fileNames1.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            for (int i = 0; i < files1.Length; i++)
            {
                string fileName = files1[i];
                XmlDocument doc = new XmlDocument();
                if (fileName != "")
                {
                    doc.Load(fileName);
                    TextReader tr = new StringReader(doc.InnerXml);
                    ds1.ReadXml(tr);
                }

                string oldfileName = files1[i].Replace("转坐标后的xml", "原始xml");
                XmlDocument doc2 = new XmlDocument();
                if (oldfileName != "")
                {
                    doc.Load(oldfileName);
                    TextReader tr = new StringReader(doc.InnerXml);
                    ds2.ReadXml(tr);
                }

                var dt1 = ds1.Tables[0];
                dt1.Columns["ModelName"].SetOrdinal(4);
                var dt2 = ds2.Tables[0];
                dt2.Columns["ModelName"].SetOrdinal(4);

                dt1.Columns.Add("modelNmamNew", typeof(string));
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    dt1.Rows[j]["modelNmamNew"] = dt2.Rows[j]["ModelName"];
                }
                dt1.Columns.Remove("ModelName");
                dt1.Columns["modelNmamNew"].ColumnName = "ModelName";

                string filePath = files1[i].Replace("转坐标后的xml", "列数据替换结果文件");
                string[] namelist = filePath.Split('\\');
                string newFileFolderPath = string.Empty;
                for (int k = 0; k < namelist.Length - 1; k++)//E:\o.cn\镇江市自规局\shp、xml转换\问题一\列数据替换结果文件\03G\主核心区现状.xml
                {
                    newFileFolderPath += namelist[k] + "\\";
                }

                if (newFileFolderPath != "")
                {
                    if (!Directory.Exists(newFileFolderPath))
                        Directory.CreateDirectory(newFileFolderPath);
                }

                string fileAllName = newFileFolderPath + namelist[namelist.Length - 1];
                if (newFileFolderPath != "")
                {
                    DataTableToXml(dt1, fileAllName);
                }                   

            }

            MessageBox.Show("succsss");
        }
        public static void DataTableToXml(DataTable vTable,string filepath)

        {

            string savePath = Application.StartupPath.ToString();

            if (!Directory.Exists(savePath))

            {

                Directory.CreateDirectory(savePath);

            }
            

            //如果文件DataTable.xml存在则直接删除

            if (File.Exists(filepath))

            {

                File.Delete(filepath);

            }

            vTable.WriteXml(filepath);

        }
    }
}
