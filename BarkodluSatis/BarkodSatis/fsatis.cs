using System;
using System.Linq;
using System.Windows.Forms;

namespace BarkodSatis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BarkodDBEntities db = new BarkodDBEntities();
        
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = tBarkod.Text.Trim();
                if (barkod.Length <= 2)
                {
                    tMiktar.Text = barkod;
                    tBarkod.Clear();
                    tBarkod.Focus();
                }
                else
                {
                    if (db.Urun.Any(a => a.Barkod == barkod))
                    {
                        var urun = db.Urun.Where(a => a.Barkod == barkod).FirstOrDefault();
                        int satirsayisi = gridSatislistesi.Rows.Count;
                        double miktar = Convert.ToDouble(tMiktar.Text);
                        bool eklenmismi = false;
                        if (satirsayisi > 0)
                        {
                            for (int i = 0; i < satirsayisi; i++)
                            {
                                if (gridSatislistesi.Rows[i].Cells["barkod"].Value.ToString() == barkod)
                                {
                                    gridSatislistesi.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(gridSatislistesi.Rows[i].Cells["Miktar"].Value);
                                    gridSatislistesi.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(gridSatislistesi.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(gridSatislistesi.Rows[i].Cells["Fiyat"].Value), 2);
                                    eklenmismi = true;


                                }
                            }
                        }
                        if (!eklenmismi)
                        {
                            gridSatislistesi.Rows.Add();
                            gridSatislistesi.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                            gridSatislistesi.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                            gridSatislistesi.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                            gridSatislistesi.Rows[satirsayisi].Cells["Birim"].Value = urun.Birim;
                            gridSatislistesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyat�;
                            gridSatislistesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                            gridSatislistesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat�, 2);
                            gridSatislistesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = urun.AlisFiyat�;
                            gridSatislistesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;


                        }
                    }
                }
                gridSatislistesi.ClearSelection();
                GenelToplam();
                
            }
        }

        private void GenelToplam()
        {
            if (gridSatislistesi.Rows.Count >0)
            {
                double toplam = 0;
                for (int i = 0; i < gridSatislistesi.Rows.Count; i++)
                {
                    toplam += Convert.ToDouble(gridSatislistesi.Rows[i].Cells["Toplam"].Value);
                }
                tGenelToplam.Text = toplam.ToString("c2");
                tBarkod.Clear();
                tBarkod.Focus();

            }
        }
    }
}

