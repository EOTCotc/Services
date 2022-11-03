using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace DID.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonHelp
    {
        /// <summary>
        /// 手机号验证
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsPhoneNum(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$");
        }
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)");
        }
        /// <summary>
        /// 证件号验证
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsCard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{7,18}$)|(^\d{17}(\d|X|x)$)");
        }
        /// <summary>
        /// 邮件验证
        /// </summary>
        /// <param name="str_mail"></param>
        /// <returns></returns>
        public static bool IsMail(string str_mail)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_mail, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 判读文件是否为图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsPicture(IFormFile file)
        {
            try
            {
                var reader = new BinaryReader(file.OpenReadStream());
                string fileClass;
                byte buffer;
                buffer = reader.ReadByte();
                fileClass = buffer.ToString();
                buffer = reader.ReadByte();
                fileClass += buffer.ToString();
                reader.Close();
                if (fileClass == "255216" || fileClass == "7173" || fileClass == "13780" || fileClass == "6677")

                //255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //使用Graphics往指定区域打马赛克
        public static Image MaSaiKeGraphics(Image sImage, int val, Rectangle rectangle)
        {
            Bitmap rBitmap = new Bitmap(sImage);
            if (rBitmap.Equals(null))
            {
                return null;
            }
            using (Graphics g = Graphics.FromImage(rBitmap))
            {
                int startX = rectangle.Left, startY = rectangle.Top;
                int fWindth = 0, fHeight = 0;
                long outvalue = 0;

                long rows = Math.DivRem(rectangle.Height, val, out outvalue);
                if (outvalue == 0) rows--;
                long columns = Math.DivRem(rectangle.Width, val, out outvalue);
                if (outvalue == 0) columns--;

                for (int y = 0; y <= rows; y++)
                {
                    startY = rectangle.Top + y * val;
                    if (startY + val > rectangle.Bottom) fHeight = rectangle.Bottom - startY;
                    else fHeight = val;

                    for (int x = 0; x <= columns; x++)
                    {
                        startX = rectangle.Left + x * val;
                        if (startX + val > rectangle.Right) fWindth = rectangle.Right - startX;
                        else fWindth = val;

                        Rectangle fillRectangle = new Rectangle(startX, startY, fWindth, fHeight);
                        Color fillColor = rBitmap.GetPixel(startX, startY);//Color.White;
                        g.FillRectangle(new SolidBrush(fillColor), fillRectangle);
                    }
                }
            }
            return rBitmap;
        }

        //使用Graphics往指定区域打马赛克
        public static Image WhiteGraphics(Image sImage, Rectangle rectangle)
        {
            Bitmap rBitmap = new Bitmap(sImage);
            if (rBitmap.Equals(null))
            {
                return null;
            }
            using (Graphics g = Graphics.FromImage(rBitmap))
            {
                Color fillColor = Color.White; //rBitmap.GetPixel(startX, startY);
                g.FillRectangle(new SolidBrush(fillColor), rectangle);
            }
            return rBitmap;
        }

        //使用Graphics给图片打马赛克
        public static Image MaSaiKeGraphics(Image image, int val)
        {
            var with = (int)(image.Width * 0.3);
            var height = image.Height/5;

            Random rm = new();
            image = MaSaiKeGraphics(image, val,
                  new Rectangle((int)(image.Width * 0.75), height * 4, (int)(image.Width * 0.25), (int)(height *0.8)));
            image = MaSaiKeGraphics(image, val,
                  new Rectangle((int)(image.Width * 0.30), height * 4, (int)(image.Width * 0.15), (int)(height * 0.8)));
            for (int i = 0; i < 6; i++)
            {
                var x = rm.Next(image.Width - with);
                var y = rm.Next(image.Height - height);
                image = MaSaiKeGraphics(image, val,
                   new Rectangle(x, y, (int)(with * 0.5), height));
            }
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        //public static Image AddWaterMark(string UId, Image image)
        //{
        //    //创建一个image对象
        //    var myGraphic = Graphics.FromImage(image);                                         //创建Graphic对象，核心的类
        //    var sourceString = $"{DateTime.Today:yyyy-MM-dd}";                               //水印内容(今天日期)
        //    var font = new Font("Arial", 23);                                                // 水印字体
        //    var size = myGraphic.MeasureString(sourceString, font);                          //水印的尺寸
        //    myGraphic.DrawString(sourceString, font, System.Drawing.Brushes.Red,             //把我们上面给水印定义好的属性放进去，并给水印添加个红色字体
        //        new PointF(image.Width - size.Width - 30, image.Height - size.Height - 30));     //计算好水印的位置（根据宽高做个减法）
        //    myGraphic.Dispose();                                                             //释放myGraphic
        //    return image;
        //}
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            //将字符串编码为字节序列
            byte[] bt = Encoding.UTF8.GetBytes(str);
            //创建默认实现的实例
            var md5 = MD5.Create();
            //计算指定字节数组的哈希值。
            var md5bt = md5.ComputeHash(bt);
            //将byte数组转换为字符串
            var builder = new StringBuilder();
            foreach (var item in md5bt)
            {
                builder.Append(item.ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 姓名处理为姓***
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetName(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var index = str.Length;
                str = str.Substring(0, 1);
                for (var i = 0; i < 2; i++)
                    str += '*';
            }
            return str;
        }

        #region
        #endregion

    }
}
