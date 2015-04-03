using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace GPRMC
{
    public partial class ForMain : Form
    {


        /// <summary>
        /// 全局变量
        /// </summary>
        string FilePath;
        //string FileName;
        StreamReader sr;
        string FileString = string.Empty;
        string[] split;
        string[] split_temp;
        List<string> split_crouse = new List<string>();
        List<string> split_speed = new List<string>();

  

        public ForMain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_Time.Text = DateTime.Now.ToString();
        }
        // 选取文件
        private void button_Read_Click(object sender, EventArgs e)
        {
            TEST();
            //Thread newThread = new Thread(new ThreadStart(TEST));
            //newThread.SetApartmentState(ApartmentState.STA);
            //newThread.Start();
            draw();
        }
        private void TEST(object obj)
        {
            split_crouse.Clear();
            split_speed.Clear();
            //ucWave_Course.ClearAllPoints();
            //ucWave_Course.Refresh();
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "*.txt|*.txt|*.*|*.*";
            openDlg.InitialDirectory = System.Environment.CurrentDirectory;
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // 获取文件路径和文件名
                FilePath = openDlg.FileName;
            }
            // 将字节流转化为string,并分割成一个个小的string整报文  
            if (FilePath != null)
            {
                sr = new StreamReader(FilePath, Encoding.Default);
                //FilePath.Close();
                FileString = sr.ReadToEnd();
                split = FileString.Split(new Char[] { '$' });
            }
            //清空

            split_crouse.Clear();
            split_speed.Clear();

            // 将split[]字符数组进行判断：是否是所需
            int index_baowen = 0;
            if (split != null) // 文件非空
            {
                split_crouse.Clear();
                split_speed.Clear();
                if (index_baowen < split.Length) //报文未处理完毕
                {
                    split_crouse.Clear();
                    split_speed.Clear();
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != string.Empty)
                        {
                            bool test1 = split[i].Substring(0, 5).Equals("GPRMC");
                            if (test1)
                            {
                                split_temp = split[i].Split(new Char[] { ',', '*' });
                                split_crouse.Add(split_temp[8]);
                                split_speed.Add(split_temp[7]);
                                index_baowen++;
                            }
                        }
                    }
                }
            }
        }
        // 报文文件-->变成一个个整体的string报文
        private void TEST()
        {
            split_crouse.Clear();
            split_speed.Clear();
            //ucWave_Course.ClearAllPoints();
            //ucWave_Course.Refresh();
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "*.txt|*.txt|*.*|*.*";
            openDlg.InitialDirectory = System.Environment.CurrentDirectory;
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // 获取文件路径和文件名
                FilePath = openDlg.FileName;
            }
            // 将字节流转化为string,并分割成一个个小的string整报文  
            if (FilePath != null)
            {
                sr = new StreamReader(FilePath, Encoding.Default);
                //FilePath.Close();
                FileString = sr.ReadToEnd();
                split = FileString.Split(new Char[] { '$' });
            }
            //清空
            
            split_crouse.Clear();
            split_speed.Clear();
            
            // 将split[]字符数组进行判断：是否是所需
            int index_baowen = 0;
            if (split != null) // 文件非空
            {
                split_crouse.Clear();
                split_speed.Clear();
                if (index_baowen < split.Length) //报文未处理完毕
                {
                    split_crouse.Clear();
                    split_speed.Clear();
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != string.Empty)
                        {
                            bool test1 = split[i].Substring(0, 5).Equals("GPRMC");
                            if (test1)
                            {
                                split_temp = split[i].Split(new Char[] { ',', '*' });
                                split_crouse.Add(split_temp[8]);
                                split_speed.Add(split_temp[7]);
                                index_baowen++;
                            }
                        }
                    }
                }
            }
            //显示报文数据量
            label_Start.Text = split_crouse.Count.ToString();
            //每次画图前先刷新
            ucWave_Course.Refresh();
            scattergramSpeed1.Refresh();
        }



        public void draw()
        {
            #region 画航向图

            //将所需航向数组存入arraylist
            for (int index = 0; index < split_crouse.Count; index++)
            {
                if (split_crouse[index] != string.Empty)
                {
                    var ff = float.Parse(split_crouse[index]);

                    ucWave_Course.AddPoint(index, ff);
                }
                else
                {
                    ucWave_Course.AddPoint(index, (float)0.0);
                }
            }
            //ucWave_Course.DrawXY();
            //ucWave_Course.ClearAllPoints();
            #endregion

            #region 画航速图
            for (int index_1 = 0; index_1 < split_speed.Count; index_1++)
            {
                if (split_speed[index_1] != string.Empty)
                {
                    var ff = float.Parse(split_speed[index_1]);

                    scattergramSpeed1.AddPoint(index_1, ff);
                }
                else
                {
                    scattergramSpeed1.AddPoint(index_1, (float)0.0);
                }
            }
            ucWave_Course.DrawXY();
            ucWave_Course.ClearAllPoints();
            scattergramSpeed1.DrawXY();
            scattergramSpeed1.ClearAllPoints();
            #endregion







        }


    }
}
