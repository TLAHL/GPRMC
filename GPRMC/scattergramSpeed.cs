using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GPRMC
{
    public partial class scattergramSpeed : UserControl
    {
        
        #region 全局变量
        /// <summary>
        /// 图g
        /// </summary>
        public Graphics g;
        /// <summary>
        /// 点列
        /// </summary>
        public Hashtable listPoint = new Hashtable();
        /// <summary>
        /// 用来绘制的点对象
        /// </summary>
        Pen pPoint = new Pen(Color.Goldenrod);
        /// <summary>
        /// 用来绘制的线对象
        /// </summary>
        Pen pline = new Pen(Color.Red);
        /// <summary>
        /// 填充颜色
        /// </summary>
        SolidBrush bString = new SolidBrush(Color.DimGray);

        Pen pXY = new Pen(Color.DimGray);
        /// <summary>
        /// 段数（9段 10条线）横线
        /// </summary>
        public int IWaterLevelMax
        {
            get;
            set;
        }
        #endregion

        #region 构造函数
        public scattergramSpeed()
        {
            InitializeComponent();
            IWaterLevelMax = 9;//纵坐标分了九份
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        }
        #endregion

        #region 主逻辑程序
        public void UcWave_Paint(object sender, PaintEventArgs e)
        {
            //g = this.CreateGraphics();

            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;           //呈现质量  图形抗锯齿
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;       
            //1.画X、Y座标轴，栅格线
            DrawXY();
        }
        #endregion
        #region 画点


        #endregion
        #region 画X、Y座标轴，栅格线
        public void DrawXY()
        {
            g = this.CreateGraphics();

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;           //呈现质量  图形抗锯齿
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;        // 文字抗锯齿 
            #region 画横向线
            float fX0 = 35, fY0 = Height - 30, fX1 = Width - 35, fY1 = 30;
            //画X轴
            g.DrawLine(pXY, fX0, fY0, fX1 + 20, fY0);	             //加20为了延伸X轴
            //画横线
            var fHeightTemp = (float)(fY0 - fY1) / IWaterLevelMax;	 //每段的长度 用来画横线
            for (int i = 0; i <= IWaterLevelMax; i++)				 // IWaterLevelMax段			 IWaterLevelMax条线
            {
                g.DrawLine(pXY, fX0 - 10, fY0 - fHeightTemp * i, fX1, fY0 - fHeightTemp * i);		//画横线
                var strTemp = string.Format("   {0}节\n", 100 * i);//一定要空两个            			//标识格式
                g.DrawString(strTemp, Font, bString, fX0 - 50, fY0 - fHeightTemp * i - 13);			//标识
                if (i < IWaterLevelMax)
                {
                    g.DrawLine(pXY, fX0 - 5, fY0 - fHeightTemp * i - fHeightTemp / 2, fX0, fY0 - fHeightTemp * i - fHeightTemp / 2);			//画短横线 只有9条
                }
            }
            #endregion

            #region 画纵向线
            //画Y轴
            g.DrawLine(pXY, fX0, fY0, fX0, fY1 - 20);		//-20为了延伸Y轴
            //画竖线
            var fWidthTemp = (float)(fX1 - fX0) / 16;		//每大段长度
            for (int j = 0; j <= 16; j++)
            {
                if (j < 16)
                {
                    for (int k = 1; k < 12; k++)
                    {
                        int iheight = 5;				    //刻度线
                        if (k == 6)
                        {
                            iheight = 10;				    //中间刻度线,要加长
                        }
                        g.DrawLine(pXY, fX0 + j * fWidthTemp + k * fWidthTemp / 12, fY0, fX0 + j * fWidthTemp + k * fWidthTemp / 12, fY0 - iheight);//画细小的刻度线
                    }
                }
                g.DrawLine(pXY, fX0 + j * fWidthTemp, fY0, fX0 + j * fWidthTemp, fY1);					  //画竖线 25条
                g.DrawString(j.ToString(), Font, bString, fX0 + j * fWidthTemp - 5, fY0 + 5);			  //标注时刻
            }
            g.DrawString("(分钟)", Font, bString, fX0 + 16 * fWidthTemp + 15, fY0 + 5);					  //标注“分钟”
            #endregion

            #region 画点
            if (arrayList != null)
            {
                for (int i = 0; i < arrayList.Count; i++)
                {
                    int ix1 = (int)arrayList[i];
                    float fx1 = ix1 * fWidthTemp / 60;   //12->60
                    float fy1 = (float)((float.Parse(listPoint[ix1].ToString()) * fHeightTemp) / 40);  //2->40
                    PointF P1 = new PointF(fX0 + fx1, fY0 - fy1);
                    if (i > 0 && arrayList.Count >= 1)
                    {
                        int ix0 = (int)arrayList[i - 1];
                        float fx0 = ix0 * fWidthTemp / 60;//12->60
                        float fy0 = (float)((float.Parse(listPoint[ix0].ToString()) * fHeightTemp) / 40);//2->40
                        PointF P0 = new PointF(fX0 + fx0, fY0 - fy0);
                        g.DrawLine(pline, P0, P1);//绘制一条连接两个 PointF 结构的线。
                        Thread.Sleep(30);
                    }
                    // 画两个点之间的小弧线，使其看起来更加平滑
                    // g.DrawEllipse(pPoint, P1.X - 1, P1.Y - 1, 2, 2);// 绘制一个由边框（该边框由一对坐标、高度和宽度指定）定义的椭圆
                }
            }
            #endregion
        }
        #endregion

        #region 添加点
        /// <summary>
        /// 使用大小可按需动态增加的数组实现 System.Collections.IList 接口。
        /// </summary
        public ArrayList arrayList;
        public void AddPoint(int iIndex, float iValue)
        {
            if (listPoint.Contains(iIndex))
            {
                listPoint[iIndex] = iValue;
            }
            else
            {
                listPoint.Add(iIndex, iValue);
            }
            arrayList = new ArrayList(listPoint.Keys);
            arrayList.Sort();
        }
        #endregion

        #region 清空所有的点
        public void ClearAllPoints()
        {
            listPoint.Clear();
            if (arrayList != null)
            {
                arrayList.Clear();
            }
        }
        #endregion

        #region 没用
        //const int size = 500;
        //double[] x = new double[size];
        //Graphics graphics = e.Graphics;
        //Pen pen = new Pen(Color.Teal);
        ////在这里采用2。 
        //int val = 2;
        //float temp = 0.0f;
        ////把画布下移100。为什么要这样做，只要你把这一句给注释掉，运行一下代码， 
        ////你就会明白是为什么？ 
        //graphics.TranslateTransform(0, 100);
        //for (int i = 0; i < size; i++)
        //{
        //    //改变32，实现正弦曲线宽度的变化。 
        //    //改100，实现正弦曲线高度的变化。
        //    x[i] = Math.Sin(2*Math.PI*i/32)*100;
        //    graphics.DrawLine(pen, i*val, temp, i*val + val/2, (float) x[i]);
        //    temp = (float) x[i];
        //}
        #endregion
    }
}
