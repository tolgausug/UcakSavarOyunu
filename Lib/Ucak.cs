using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace UcakSavarOyunu.Lib
{
    public class Ucak : OyunBase, IHareketEdebilir
    {
        private Point konum;
       
        
        
        public Ucak(Point konum)
        {
            this.konum = konum;
            ResimKutusu = new PictureBox()
            {
                Size = new Size(52, 44),
                Image = Properties.Resources.ucak,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = konum
            };
        }

       

        public void HareketEt(Yonler yon)
        {
            if (yon == Yonler.Asagi)
            {
                Point point = new Point()
                {
                    X = ResimKutusu.Location.X,
                    Y = ResimKutusu.Location.Y + 5
                };
                this.ResimKutusu.Location = point;
            }
        }
        
    }
}
