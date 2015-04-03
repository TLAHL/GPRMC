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




            #region 画航速图

            ////*****************************************************
            ////将所需航速数组存入arraylist
            //for (int index = 0; index < split_crouse.Count; index++)
            //{
            //    if (split_crouse[index] != string.Empty)
            //    {
            //        var ff = float.Parse(split_crouse[index]);
            //        ucWave_Course.AddPoint(index, ff);
            //    }
            //    else
            //    {
            //        ucWave_Course.AddPoint(index, (float)0.0);
            //    }
            //}
            ////画航向图
            //ucWave_Course.DrawXY();
            #endregion


        }


        #region WUYONG
        //int i = 0;
            //var temp_width = (int)((ucWave_Course.Width - 70) / (16.0 * 60.0));//每一个点的X轴位移
            ////将GroundCourse转化为float型
            //if (split_crouse[i] != string.Empty)
            //{
            //    GroundCrouse = float.Parse(split_crouse[i]);
            //}
            //var temp_Height = (float)(((ucWave_Course.Height - 60) / 360.0) * temp_width);//每一次右移的宽度
            //float true_height = (float)(ucWave_Course.Height - (GroundCrouse + 30));//每一个点的转化高度
            //#region
            //////g.DrawRectangle(pen,new Rectangle(iInex,iInex,300,400));
            //if (iInex == 0)
            //{
            //    ucWave_Course.ClearAllPoints();
            //}
            //if (iInex == 961)
            //{
            //    ucWave_Course.ClearAllPoints();
            //    iInex = 0;
            //}
            //////float IInex = float.Parse(iInex.ToString());
            //////ucWave_Course.DrawPoint(iInex,true_height);   
            //ucWave_Course.AddPoint(iInex, true_height);
            //ucWave_Course.Refresh();
            //#endregion
            ////******************************************************************************
            //iInex += temp_width;
            //i++;
           
        
        //string GroundSpeed;
        //string GroundCourse;
        //string str = "GPRMC";
        //int index = 0;
        //int temp_index = 0;
        //int iInex = 35;

        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    //if (split != null)  // 文件中的有效报文不为空
        //    //{
        //    //    // 报文没有处理完
        //    //    if (index < split.Length)
        //    //    {
        //    //        split_temp = split[index].Split(new Char[] { ',', '*' });
        //    //        if (split_temp[0] == str)//判断报文类型
        //    //        {
        //    //            // 获取了时间信息（是否有判空需求）
        //    //            string ymdhmsTime = string.Concat(split_temp[9], "-", split_temp[1]);
        //    //            label_End.Text = DateTime.ParseExact(ymdhmsTime, "ddMMyy-hhmmss.ff", CultureInfo.InvariantCulture).ToString();
        //    //            // 获取了航速和航向的字段,用于画点
        //    //            GroundSpeed = split_temp[7];
        //    //            GroundCourse = split_temp[8];
        //    //            var temp_width = (int)((ucWave_Course.Width - 70) / (16.0 * 60.0));//每一个点的X轴位移

        //    //            #region 航向
        //    //            if (GroundCourse != string.Empty) // 不能传入空字符串
        //    //            {
        //    //                // 将GroundCourse转化为float型
        //    //                float temp_course = float.Parse(GroundCourse);
        //    //                var temp_Height = (float)(((ucWave_Course.Height - 60) / 360.0) * temp_course);//每一次右移的宽度
        //    //                float true_height = (float)(ucWave_Course.Height - (temp_Height + 30));//每一个点的转化高度





        //    //                #region
        //    //                ////g.DrawRectangle(pen,new Rectangle(iInex,iInex,300,400));
        //    //                ////if (iInex == 35)
        //    //                ////{
        //    //                ////    ucWave_Course.ClearAllPoints();
        //    //                ////}
        //    //                ////if (iInex == 996)
        //    //                ////{
        //    //                ////    ucWave_Course.ClearAllPoints();
        //    //                ////    iInex = 35;
        //    //                ////}
        //    //                ////float IInex = float.Parse(iInex.ToString());
        //    //                ////ucWave_Course.DrawPoint(iInex,true_height);   
        //    //                //ucWave_Course.AddPoint(iInex, true_height);
        //    //                //ucWave_Course.Refresh();
        //    //                #endregion
        //    //                //******************************************************************************
        //    //                iInex += temp_width;
        //    //                label_Start.Text = temp_index.ToString();
        //    //                temp_index++;
        //    //            }
        //    //            else
        //    //            {
        //    //                // 如果该点为空，必须向后移动相应的位置，确保不影响后续的点的坐标
        //    //                iInex += temp_width;
        //    //                temp_index++;
        //    //                label_Start.Text = temp_index.ToString();

        //    //            }
        //    //            #endregion

        //    //        }
        //    //        index++;
        //    //    }
        //    //    // 如果报文处理完了
        //    //    else
        //    //    {
        //    //        ucWave_Course.DrawXY();
        //    //        ucWave_Course.Refresh();
        //    //        Thread.Sleep(5000);
        //    //    }
        //    //}

        //}


        #endregion


    }
}
