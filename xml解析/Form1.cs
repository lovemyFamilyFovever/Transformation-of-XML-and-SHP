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

                    DataTableToString(dt,fileName);
                }
            }
            MessageBox.Show("数据导出","转换成功!");
        }

        public static string DataTableToString(DataTable dt,string filePath)
        {
            filePath=filePath.Replace("xml","txt");
            System.Text.StringBuilder builder = new StringBuilder();
            foreach(DataRow dr in dt.Rows)
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

            string sPath = "";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择所有文件存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {

                sPath = folder.SelectedPath;
            }

            string[] files = Directory.GetFiles(sPath, "*.xml", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                this.textBox2.AppendText(files[i] + "\r\n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string sPath = "";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择所有文件存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {

                sPath = folder.SelectedPath;
            }

            string[] files = Directory.GetFiles(sPath, "*.xml", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                this.textBox3.AppendText(files[i] + "\r\n");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text != ""&& this.textBox3.Text != "")
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
            for (int i = 0; i < files1.Length; i++)
            {
                string fileName = files1[i];
                XmlDocument doc = new XmlDocument();
                if (fileName != "")
                {
                    doc.Load(fileName);

                    
                    TextReader tr = new StringReader(doc.InnerXml);
                    ds1.ReadXml(tr);

                    DataTable dt = ds.Tables[0];
                    dt.Columns["ModelName"].SetOrdinal(4);

                }
            }

            //原始的xml
            string fileNames2 = textBox3.Text;
            string[] files2 = fileNames2.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            DataSet ds2 = new DataSet();
            for (int i = 0; i < files2.Length; i++)
            {
                string fileName = files2[i];
                XmlDocument doc = new XmlDocument();
                if (fileName != "")
                {
                    doc.Load(fileName);

                    TextReader tr = new StringReader(doc.InnerXml);
                    ds2.ReadXml(tr);

                    DataTable dt = ds2.Tables[0];
                    dt.Columns["ModelName"].SetOrdinal(4);

                    //DataTableToString(dt, fileName);
                }
            }
            

            for(int i = 0; i < ds1.Tables.Count; i++)
            {
                var dt1 = ds1.Tables[i];
                var dt2 = ds2.Tables[i];

                object[] obj = new object[1];
                for (int j = 0; i < dt1.Rows.Count; i++)
                {
                    DataTable1.Rows[i].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
        }

    }
}
