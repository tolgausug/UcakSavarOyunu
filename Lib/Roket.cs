using System.Drawing;
using System.Windows.Forms;

namespace UcakSavarOyunu.Lib
{
    public class Roket : OyunBase, IHareketEdebilir
    {
        
        public Roket(Point konum)
        {
            ResimKutusu = new PictureBox()
            {
                Size = new Size(28, 36),
                Image = Properties.Resources.mermi1,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = konum
            };
        }
        public void HareketEt(Yonler yon)
        {

            if (yon == Yonler.Yukari)
            {
                Point point = new Point()
                {
                    X = ResimKutusu.Location.X,
                    Y = ResimKutusu.Location.Y - 5
                };
                ResimKutusu.Location = point;
            }
        }
    }
}
