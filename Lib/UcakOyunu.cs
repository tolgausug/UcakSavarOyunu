using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace UcakSavarOyunu.Lib
{
    public class UcakOyunu
    {
        private Timer tmrUretici, tmrMermi, tmrKontrol, tmrUcak;
        public bool OyunDurduMu { get; set; } = false;
        public int Skor { get; set; } = 0;
        public bool durduMu { get; set; } = false;
        public Ucaksavar Ucaksavar { get; set; }
        public List<Ucak> Ucaklar { get; set; } = new List<Ucak>();
        public List<Konumlar> roketKonumları { get; set; } = new List<Konumlar>();
        public List<Roket> Roketler { get; set; } = new List<Roket>();

        private ContainerControl tasiyici;
        public UcakOyunu(ContainerControl tasiyici)
        {
            this.tasiyici = tasiyici;
            this.Ucaksavar = new Ucaksavar(tasiyici);
            this.Ucaksavar.ResimKutusu.Click += ucakresim_click;

            tmrMermi = new Timer();
            tmrMermi.Interval = 30;
            tmrMermi.Tick += TmrMermi_Tick;
            tmrMermi.Start();

            

            tmrUretici = new Timer();
            tmrUretici.Interval = 2000;
            tmrUretici.Tick += TmrUretici_Tick;
            tmrUretici.Start();

            tmrUcak = new Timer();
            tmrUcak.Interval = 200;
            tmrUcak.Tick += TmrUcak_Tick;
            tmrUcak.Start();
            
            tmrKontrol = new Timer();
            tmrKontrol.Interval = 5;
            tmrKontrol.Tick += TmrKontrol_Tick;
            tmrKontrol.Start();
        }
        
      
        

        private void TmrKontrol_Tick(object sender, EventArgs e)
        {
            if (durduMu)
            {
                
                tmrMermi.Stop();
                tmrUcak.Stop();
                tmrUretici.Stop();
                
            }
            else
            {
                tmrMermi.Start();
                tmrUcak.Start();
                tmrUretici.Start();

            }
            roketKonumları.Clear();
            foreach (var ucak in Ucaklar)
            {
                Rectangle ur = new Rectangle();
                Rectangle mr = new Rectangle();
                bool vurdumu = false;
                if (ucak.ResimKutusu.Location.Y + ucak.ResimKutusu.Height > tasiyici.Height - 70)
                {
                    OyunDurduMu = true;
                    tmrKontrol.Stop();
                    tmrMermi.Stop();
                    tmrUcak.Stop();
                    tmrUretici.Stop();
                }
                foreach (var roket in this.Ucaksavar.Roketler)
                {
                    ur.X = ucak.ResimKutusu.Left;
                    ur.Y = ucak.ResimKutusu.Top;
                    ur.Height = ucak.ResimKutusu.Height;
                    ur.Width = ucak.ResimKutusu.Width;

                    mr.X = roket.ResimKutusu.Left;
                    mr.Y = roket.ResimKutusu.Top;
                    mr.Height = roket.ResimKutusu.Height;
                    mr.Width = roket.ResimKutusu.Width;
                    if (ur.IntersectsWith(mr))
                    {
                        tasiyici.Controls.Remove(ucak.ResimKutusu);
                        tasiyici.Controls.Remove(roket.ResimKutusu);
                        Ucaklar.Remove(ucak);
                        Ucaksavar.Roketler.Remove(roket);
                        Skor++;
                        vurdumu = true;
                        SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.bomb_small);
                        soundPlayer.Play();

                        if (Skor % 10 == 0 && Skor > 1 && tmrUretici.Interval > 2)
                            tmrUretici.Interval -= 1;

                        break;
                    }
                }
                if (vurdumu) break;
            }

            foreach (var item in this.Ucaksavar.Roketler)
            {
                if (item.ResimKutusu.Location.Y < 0)
                {
                    this.Ucaksavar.Roketler.Remove(item);
                    tasiyici.Controls.Remove(item.ResimKutusu);
                    break;
                }
            }
            
            foreach (var item in Ucaksavar.Roketler)
            {

                Konumlar yenikonum = new Konumlar()
                {
                    x = item.ResimKutusu.Location.X.ToString(),
                    y = item.ResimKutusu.Location.Y.ToString(),
                    skor = this.Skor.ToString()
                    };
                roketKonumları.Add(yenikonum);
                
            }
            foreach (var item in Ucaklar)
            {

                Konumlar yenikonum = new Konumlar()
                {
                    z = item.ResimKutusu.Location.X.ToString(),
                    c = item.ResimKutusu.Location.Y.ToString(),
                    skor = this.Skor.ToString()
                };
                roketKonumları.Add(yenikonum);

            }


        }
        
        Random rnd = new Random();
        

        private void TmrUretici_Tick(object sender, EventArgs e)
        {
            Point point = new Point()
            {
                X = rnd.Next(60, tasiyici.Width - 120),
                Y = 0
            };
            Ucak ucak = new Ucak(point);
            ucak.ResimKutusu.Click += ucakresim_click;
            Ucaklar.Add(ucak);
            
            tasiyici.Controls.Add(ucak.ResimKutusu);
            
        }
        
        private void ucakresim_click(object sender, EventArgs e)
        {
            this.Ucaksavar.AtesEt();
        }

        private void TmrUcak_Tick(object sender, EventArgs e)
        {
            foreach (var item in Ucaklar)
            {
                item.HareketEt(Yonler.Asagi);
                
            }
        }

        private void TmrMermi_Tick(object sender, EventArgs e)
        {
            foreach (var item in this.Ucaksavar.Roketler)
            {
                item.HareketEt(Yonler.Yukari);
            }
        }
        
    }
}
