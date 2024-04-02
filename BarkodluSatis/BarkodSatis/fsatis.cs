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
                        UrunGetirListeye(urun, barkod, Convert.ToDouble(tMiktar.Text));
                    }
                    else
                    {
                        int onek = Convert.ToInt32(barkod.Substring(0, 2));
                        if (db.Terazi.Any(a => a.TeraziOnEk == onek)) 
                        {
                            string teraziurunno = barkod.Substring(2, 5);
                           if (db.Urun.Any(a=> a.Barkod == teraziurunno))
                            {
                                var urunterazi = db.Urun.Where(a => a.Barkod == teraziurunno).FirstOrDefault();
                                double miktarkg = Convert.ToDouble(barkod.Substring(7, 5))/1000;
                                UrunGetirListeye(urunterazi, teraziurunno, miktarkg);
                            }
                            else
                            {
                                Console.Beep(900, 2000);
                                MessageBox.Show("Kg ürün ekleme sayfasý");
                            }
                        }
                        else
                        {
                            Console.Beep(900, 2000);
                            MessageBox.Show("Normal ürün ekleme sayfasý");
                        }
                    }
                }
                gridSatislistesi.ClearSelection();
                GenelToplam();
                
            }
        }

        private void UrunGetirListeye(Urun urun,string barkod,double miktar)
        {
            int satirsayisi = gridSatislistesi.Rows.Count;
           
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
                gridSatislistesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyatý;
                gridSatislistesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                gridSatislistesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyatý, 2);
                gridSatislistesi.Rows[satirsayisi].Cells["AlisFiyati"].Value = urun.AlisFiyatý;
                gridSatislistesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;
            } 
        }

        private void GenelToplam()
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

        private void gridSatislistesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 9)
            {
                gridSatislistesi.Rows.Remove(gridSatislistesi.CurrentRow);
                gridSatislistesi.ClearSelection();
                GenelToplam();
                tBarkod.Focus();
            }
        }
    }
}

