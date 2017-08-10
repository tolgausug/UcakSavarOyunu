using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using UcakSavarOyunu.Lib;

namespace UcakSavarOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        UcakOyunu oyun;
        public List<Ucak> Ucaklar { get; set; } = new List<Ucak>();
        public List<Konumlar> roketKonumları { get; set; } = new List<Konumlar>();
        public List<Roket> Roketler { get; set; } = new List<Roket>();
        ListBox lst;

        Label lbl = new Label();
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                oyun?.Ucaksavar.HareketEt(Yonler.Saga);
            else if (e.KeyCode == Keys.Left)
                oyun?.Ucaksavar.HareketEt(Yonler.Sola);
            else if (e.KeyCode == Keys.Space)
                oyun?.Ucaksavar.AtesEt();
            else if (e.KeyCode == Keys.Enter)
            {
   
                if (oyun == null)
                {
                    this.Controls.Clear();
                    this.Controls.Add(menuStrip1);
                    oyun = new UcakOyunu(this);
                    timer1.Start();
                }
            }
            
            if (e.KeyCode == Keys.P)
            {
                if (oyun.durduMu == true)
                {
                    timer1.Start();
                    oyun.durduMu = false;

                }
                else
                {
                    lbl.Text = "Oyun Duraklatıldı. Devam Etmek İçin P'ye basınız..";
                    this.Text = lbl.Text;
                    timer1.Stop();
                    if (oyun != null)
                        oyun.durduMu = true;
                    else
                    {
                        return;
                    }
                }
            }
            if (e.KeyCode == Keys.W)
            {
                if (oyun != null)
                {
                    timer1.Start();
                    oyun.durduMu = false;
                }
                else { return; }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = $"Skor: {oyun.Skor} Toplam Roket: {oyun.Ucaksavar.Roketler.Count} Toplam Uçak: {oyun.Ucaklar.Count}";
            if (oyun.OyunDurduMu)
            {
                timer1.Stop();
                DialogResult cevap = MessageBox.Show($"Oyun bitti. Skor: {oyun.Skor}\nYeniden başlamak istiyor musun?", "Kaybettin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes)
                {
                    this.Controls.Clear();
                    oyun = new UcakOyunu(this);
                    timer1.Start();
                }
                else
                {
                    Application.Exit();
                }
            }
           
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (oyun?.durduMu == true) return;
            //this.Text = $"{e.Location}";
            oyun?.Ucaksavar.HareketEt(e.Location);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (oyun?.durduMu == true) return;
            oyun?.Ucaksavar.AtesEt();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oyun.durduMu = true;
            lbl.Text = "Oyun Duraklatıldı. Devam Etmek İçin P'ye basınız..";
            this.Text = lbl.Text;
            timer1.Stop();
            dosyaKaydet.Filter = "XML Format | *.xml";
            dosyaKaydet.FileName = string.Empty;
            dosyaKaydet.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dosyaKaydet.ShowDialog() == DialogResult.OK)
            {
                oyun.durduMu = true;
                timer1.Stop();
                lbl.Text = "Oyun Duraklatıldı. Devam Etmek İçin P'ye basınız..";
                this.Text = lbl.Text;
                oyun.durduMu = true;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Konumlar>));
                TextWriter writer = new StreamWriter(dosyaKaydet.FileName);
                xmlSerializer.Serialize(writer, oyun.roketKonumları);
                lst = new ListBox();
                lst.Items.Add(dosyaKaydet.FileName);
                this.Controls.Add(lst);
                writer.Close();
                writer.Dispose();
                MessageBox.Show($"Oyun {dosyaKaydet.FileName} adresine başarıyla kaydedildi");
            }
        }

        private void yükleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oyun.durduMu = true;
            lbl.Text = "Oyun Duraklatıldı. Devam Etmek İçin P'ye basınız..";
            this.Text = lbl.Text;
            timer1.Stop();
            dosyaAc.Title = "Bir Kisi XML dosyasını seçiniz";
            dosyaAc.Filter = "XML Format | *.xml";
            dosyaAc.Multiselect = false;
            dosyaAc.FileName = string.Empty;
            dosyaAc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dosyaAc.ShowDialog() == DialogResult.OK)
            { 
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Konumlar>));
                TextReader reader = new StreamReader(dosyaAc.FileName);

                
                oyun = new UcakOyunu(this);
                oyun.durduMu = false;
                
                
        oyun.roketKonumları.AddRange((List<Konumlar>)xmlSerializer.Deserialize(reader));
                this.Controls.Clear();
                this.Controls.Add(menuStrip1);

                foreach (var item in oyun.roketKonumları)
                {
                    Point pnt = new Point()
                    {
                        X = Convert.ToInt32(item.x),
                        Y = Convert.ToInt32(item.y)
                    };
                    Roket rkt = new Roket(pnt);
                    rkt.ResimKutusu.Image = Properties.Resources.mermi1;
                    this.Controls.Add(rkt.ResimKutusu);
                    oyun.Ucaksavar.Roketler.Add(rkt);
                    oyun.Skor =Convert.ToInt32(item.skor);

                }

                foreach (var item in oyun.roketKonumları)
                {
                    Point pnt = new Point()
                    {
                        X = Convert.ToInt32(item.z),
                        Y = Convert.ToInt32(item.c)
                    };
                    Ucak uck = new Ucak(pnt);
                    uck.ResimKutusu.Image = Properties.Resources.ucak;
                    this.Controls.Add(uck.ResimKutusu);
                    oyun.Ucaklar.Add(uck);

                }
                
                this.Controls.Add(oyun.Ucaksavar.ResimKutusu);
                reader.Close();
                reader.Dispose();

                //timer1.Start();
                lbl.Text = "Oyun Duraklatıldı. Devam Etmek İçin P'ye basınız..";
                this.Text = lbl.Text;
                oyun.durduMu = true;
                MessageBox.Show($"Oyun {dosyaAc.FileName} adresinden başarıyla yüklendi");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
