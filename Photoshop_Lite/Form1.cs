using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Photoshop_Lite
{
    public partial class Form1 : Form
    {
        private Bitmap image = null;                                    //Создаем заготовки для изображений
        private Bitmap image_2 = null;
        private Bitmap Image_Out = null;
        
        public Form1()
        {
            InitializeComponent();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            image_2 = new Bitmap(pictureBox2.Width,pictureBox2.Height);
            Image_Out = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            pictureBox2.Image = image_2;
            pictureBox1.Image = image;
            pictureBox3.Image = Image_Out;
        }

        private void button2_Click(object sender, EventArgs e)      //функция добавления изображение с открытием диалогового окна
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "..\\..\\Images";
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (image_2 != null)
                {
                    pictureBox2.Image = null;
                    image_2.Dispose();
                }

                image_2 = new Bitmap(openFileDialog.FileName);
                pictureBox2.Image = image_2;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;         //Сжатие изображения до размеров picturebox
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)        //магия интерфейса
        {
            if (maskbtn.Checked == true)
            {
                groupBox1.Visible = true;
                Pryamoug.Visible = true;
                CircleMaskBtn.Visible = true;
                CubeBtn.Visible = true;
                pictureBox2.Visible = false;
                OpenScndBtn.Visible = false;
            }
            else
            {
                groupBox1.Visible = false;
                Pryamoug.Visible = false;
                CircleMaskBtn.Visible = false;
                CubeBtn.Visible = false;
                CircleMaskBtn.Checked = false;
                CubeBtn.Checked = false;
                Pryamoug.Checked = false;
                pictureBox2.Visible = true;
                OpenScndBtn.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            
        }

        private void button1_Click(object sender, EventArgs e)          //добавление изображения во второй picturebox с открытием диалогового окна 
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "..\\..\\Images";
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (image != null)
                {
                    pictureBox1.Image = null;
                    image.Dispose();
                }

                image = new Bitmap(openFileDialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                
            }
        }

        private void button4_Click(object sender, EventArgs e)          //обработка изображения
        {

            Color i1, i2;
            var w = image.Width;
            var h = image.Height;
            var w_2 = image_2.Width;
            var h_2 = image_2.Height;
            int r = 0, g = 0, b = 0;
            Bitmap img_out;
            if (w * h > w_2 * h_2)                                  //проверка, какое изображение больше и растяжение меньшего до размеров большего
            {
                img_out = new Bitmap(w, h);
                image_2 = new Bitmap(image_2, w, h);
            }
            else
            {
                img_out = new Bitmap(w_2, h_2);
                image = new Bitmap(image, w_2, h_2);
                w = w_2;
                h = h_2;
            }

            

                    if (sumbtn.Checked == true)     //сумма пикселей двух изображений
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        var ptx_2 = image_2.GetPixel(j, i);

                        r = ptx.R + ptx_2.R;
                        g = ptx.G + ptx_2.G;
                        b = ptx.B + ptx_2.B;

                        r = Prove(r);                           //проверемя не превышает ли значение лимит от 0 до 255
                        g = Prove(g);
                        b = Prove(b);

                        ptx = Color.FromArgb(r, g, b);          //рисование итогового изображения по пикселям
                        img_out.SetPixel(j, i, ptx);
                    }
                }
            }

            if (multipbtn.Checked == true)          //умножение двух изображений
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        var ptx_2 = image_2.GetPixel(j, i);

                        r = (ptx.R * ptx_2.R) / 255;
                        g = (ptx.G * ptx_2.G) / 255;
                        b = (ptx.B * ptx_2.B) / 255;

                        r = Prove(r);               //проверемя не превышает ли значение лимит от 0 до 255
                        g = Prove(g);
                        b = Prove(b);

                        ptx = Color.FromArgb(r, g, b);
                        img_out.SetPixel(j, i, ptx);
                    }
                }
            }

            if (middlebtn.Checked == true)          //среднее арифметическое двух изображений
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        var ptx_2 = image_2.GetPixel(j, i);

                        r = (ptx.R + ptx_2.R) / 2;
                        g = (ptx.G + ptx_2.G) / 2;
                        b = (ptx.B + ptx_2.B) / 2;

                        r = Prove(r);
                        g = Prove(g);
                        b = Prove(b);

                        ptx = Color.FromArgb(r, g, b);
                        img_out.SetPixel(j, i, ptx);
                    }
                }
            }

            if (minbtn.Checked == true)         //нахождение минимума двух изображений
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        var ptx_2 = image_2.GetPixel(j, i);

                        r = min(ptx.R, ptx_2.R);
                        g = min(ptx.G, ptx_2.G);
                        b = min(ptx.B, ptx_2.B);

                        ptx = Color.FromArgb(r, g, b);
                        img_out.SetPixel(j, i, ptx);
                    }
                }
            }

            if (maxbtn.Checked == true)             //максимум двух изображений
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        var ptx_2 = image_2.GetPixel(j, i);

                        r = max(ptx.R, ptx_2.R);
                        g = max(ptx.G, ptx_2.G);
                        b = max(ptx.B, ptx_2.B);

                        ptx = Color.FromArgb(r, g, b);
                        img_out.SetPixel(j, i, ptx);
                    }
                }
            }

            if (GistBtn.Checked == true)
            {
                int[] N = new int[256];
                int c = 0;

                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        var ptx = image.GetPixel(j, i);
                        c = (ptx.R + ptx.G + ptx.B) / 3;
                        N[c]++;
                    }
                }
                int maxN = N.Max<int>();
                var PBH = pictureBox4.Height;
                var k =  Convert.ToDouble( PBH)/ Convert.ToDouble( maxN);
                using Graphics graf = pictureBox4.CreateGraphics();
                Pen pen = new Pen(Color.Black, 3);
                graf.Clear(Color.White);
                for (int M = 0; M < 256; M++)
                {

                    graf.DrawLine(pen,M ,pictureBox4.Height-1 ,M ,pictureBox4.Height-1-(float)Math.Ceiling(Convert.ToDouble(N[M]) * k));
                }

            }

            if (maskbtn.Checked == true)            //наложение масок
            {
                pictureBox2.Visible = false;
                OpenScndBtn.Visible = false;
                var SqSize = 0;                     //Размер квадратной маски
                var SqW = 0;                        //Ширина прямоугольной маски
                var SqH = 0;                        //Высота прямоугольной маски
               // MaskImage = new Bitmap("C:\\Users\\mihap\\source\\repos\\Photoshop_Lite\\Photoshop_Lite\\Images\\Circle.png");  
                //MaskImage = new Bitmap(MaskImage, w, h);
                if (CircleMaskBtn.Checked == true)
                {
                    int R=0;
                    if (w > h)
                        R = h / 2;
                    else
                        R = w / 2;
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            var ptx = image.GetPixel(j, i);
                            if (Math.Pow((i-h/2),2)+ Math.Pow((j - w / 2), 2) > R*R)
                                ptx = Color.FromArgb(0, 0 ,0);
                            img_out.SetPixel(j, i, ptx);
                        }
                    }
                }
                if (CubeBtn.Checked == true)        //Квадартная маска(хз почему "Cube", просто по приколу)
                {
                    if (w > h)                      //Чтобы маска нормально накладывалась определяем, что у изображения больше, ширина или высота
                        SqSize = h/2;
                    else
                        SqSize = w / 2;
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)         //Закрашиваем изображение черными пикселями за границей маски
                        {
                            var ptx = image.GetPixel(j, i);
                            if (j >= (w / 2) + SqSize/2) 
                                ptx = Color.FromArgb(0, 0, 0);
                            if (j <= (w / 2) - SqSize/2)
                                ptx = Color.FromArgb(0, 0, 0);
                            if (i >= (h / 2) + SqSize/2)
                                ptx = Color.FromArgb(0, 0, 0);
                            if (i <= (h / 2) - SqSize/2)
                                ptx = Color.FromArgb(0, 0, 0);


                            img_out.SetPixel(j, i, ptx);
                        }
                    }
                }
                if (Pryamoug.Checked == true)               //прямоугольная маска
                {
                    SqW = w / 2;                            //маска по ширине и высоте в 2 раза меньше изображения
                    SqH = h / 2;
                    var temp = 0;
                    if (SqH > SqW)                          //чтобы маска всегда была вертикальной, проверям широкое изображение или высокое
                    {
                        temp = SqW;
                        SqW = SqH;
                        SqH = temp;
                    }

                    for (int i = 0; i < h; i++)             //Закрашиваем изображение черными пикселями за границе маски
                    {
                        for (int j = 0; j < w; j++)
                        {
                            var ptx = image.GetPixel(j, i);
                            if (j >= (w / 2) + SqW / 2)
                                ptx = Color.FromArgb(0, 0, 0);
                            if (j <= (w / 2) - SqW / 2)
                                ptx = Color.FromArgb(0, 0, 0);
                            if (i >= (h / 2) + SqH / 2)
                                ptx = Color.FromArgb(0, 0, 0);
                            if (i <= (h / 2) - SqH / 2)
                                ptx = Color.FromArgb(0, 0, 0);


                            img_out.SetPixel(j, i, ptx);
                        }
                    }
                }
            }
            Color ptx3,ptx4;
            Bitmap img_out_copy = new Bitmap(img_out);                  //копия изображения, чтобы помнить оригинпльные цвета
            for (int i = 0; i < h; i++)                                 //тут мы выбираем цветовые каналы для итогового изображения(стремный код,знаю)
            {
                for (int j = 0; j < w; j++)
                {
                    ptx3 = img_out.GetPixel(j, i);
                    ptx4 = img_out_copy.GetPixel(j, i);
                    if (RGB.Checked==true)
                    {
                        r = ptx4.R;
                        g = ptx4.G;
                        b = ptx4.B;
                    }
                    if(R.Checked==true)
                    {
                        r = ptx4.R;
                        g = 0;
                        b = 0;
                    }
                    if(G.Checked==true)
                    {
                        r = 0;
                        g = ptx4.G;
                        b = 0;
                    }
                    if(B.Checked==true)
                    {
                        r = 0;
                        g = 0;
                        b = ptx4.B;
                    }
                    if(RG.Checked==true)
                    {
                        r = ptx4.R;
                        g = ptx4.G;
                        b = 0;
                    }
                    if(GB.Checked==true)
                    {
                        r = 0;
                        g = ptx4.G;
                        b = ptx4.B;
                    }
                    if(RB.Checked==true)
                    {
                        r = ptx4.R;
                        g = 0;
                        b = ptx4.B;
                    }
                    ptx3 = Color.FromArgb(r, g, b);
                    img_out.SetPixel(j, i, ptx3);
                }
                
            }
            pictureBox3.Image = img_out;                                //рисуем итогове изображение в третьем picture box и небрежно подгоняем его под размеры самого бокса
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Refresh();
            Image_Out = img_out;

            static int max(int x, int y)            //функция для определения максимума
            {
                int z=0;
                if (x > y)
                    z = x;
                else
                    z = y;
                return z;
            }
                
            static int min(int x, int y)            //ф-ция для опредления минимума
            {
                int z = 0;
                if (x < y)
                    z = x;
                else
                    z = y;
                return z;
            }

            static int Prove(int x)                 //проверка, чтобы значение пискеля было от 0 до 255
            {
                if (x > 255) { x = 255; return x; }
                else
                    if (x < 0) { x = 0; return x; }
                else return x;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           
            
        }

        private void button3_Click_1(object sender, EventArgs e)        //сохранение изображения
        {
            using SaveFileDialog saveFileFialog = new SaveFileDialog();
            saveFileFialog.InitialDirectory = "..\\..\\Result";
            saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveFileFialog.RestoreDirectory = true;

            if (saveFileFialog.ShowDialog() == DialogResult.OK)
            {
                if ( Image_Out!= null)
                {
                    Image_Out.Save(saveFileFialog.FileName);
                }
            }
            
        }

        private void GistBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (GistBtn.Checked == true)
            {
                pictureBox2.Visible = false;
                OpenScndBtn.Visible = false;
            }
            else
            {
                
                pictureBox2.Visible = true;
                OpenScndBtn.Visible = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void G_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RGB_CheckedChanged(object sender, EventArgs e)
        {

        }

        static void ChannelRGB()
        {

        }
        static void ChannelR()
        {

        }
        static void ChannelG()
        {

        }
        static void ChannelB()
        {

        }
        static void ChannelRG()
        {

        }
        static void ChannelGB()
        {

        }
        static void ChannelRB()
        {

        }
    }
}
