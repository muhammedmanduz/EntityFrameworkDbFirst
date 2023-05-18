using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace EntityOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities(); //modele ulasmak için kullandıgımız sınıf
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-60QTL3H\SQLEXPRESS;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select *from tbldersler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;




        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = db.TBLOGENCİ.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;

        }

        private void BtnNotlListesi_Click(object sender, EventArgs e)
        {
            var query = from item in db.TBLNOTLAR
                        select new
                        {
                            item.NOTID,
                            item.TBLOGENCİ.AD,
                            item.TBLOGENCİ.SOYAD,
                            item.TBLDERSLER.DERSAD,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA
                        }
                                     ;
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            TBLOGENCİ t = new TBLOGENCİ();
            t.AD = TxtAd.Text;
            t.SOYAD = TxtSoyad.Text;
            db.TBLOGENCİ.Add(t);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Eklenmiştir");

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrId.Text);
            var x = db.TBLOGENCİ.Find(id);

            db.TBLOGENCİ.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Silinmiştir");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(TxtOgrId.Text);
            var x = db.TBLOGENCİ.Find(id);


            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOGRAF = TxtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci Güncellenmiştir ");


        }

        private void BtnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();

        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOGENCİ.Where(x => x.AD == TxtAd.Text || x.SOYAD == TxtSoyad.Text).ToList();
        }

        private void TxtAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = TxtAd.Text;
            var degerler = from item in db.TBLOGENCİ
                           where item.AD.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();

        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                List<TBLOGENCİ> listele1 = db.TBLOGENCİ.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = listele1;
            }
            if (radioButton2.Checked == true)
            {
                List<TBLOGENCİ> liste2 = db.TBLOGENCİ.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = liste2;
            }
            if (radioButton3.Checked == true)
            {
                List<TBLOGENCİ> liste3 = db.TBLOGENCİ.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }

            if (radioButton4.Checked == true)
            {
                List<TBLOGENCİ> liste4 = db.TBLOGENCİ.Where(x => x.ID == 5).ToList();
                dataGridView1.DataSource = liste4;
            }
            if (radioButton5.Checked == true)
            {
                List<TBLOGENCİ> liste5 = db.TBLOGENCİ.Where(p => p.AD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton6.Checked == true)
            {
                List<TBLOGENCİ> liste6 = db.TBLOGENCİ.Where(p => p.AD.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;

            }
            if (radioButton7.Checked == true)
            {
                bool deger = db.TBLOGENCİ.Any();
                MessageBox.Show(deger.ToString());
            }
            if (radioButton8.Checked == true)
            {
                int toplam = db.TBLOGENCİ.Count();
                MessageBox.Show(toplam.ToString(), "Toplam Öğrenci Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (radioButton9.Checked == true)
            {
                var toplam = db.TBLNOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show("Toplam sınav Puanı " + toplam.ToString());
            }
            if (radioButton10.Checked == true)
            {
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);
                MessageBox.Show("1.Sınavın ortalaması :" + ortalama.ToString());

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            var sorgu = from d1 in db.TBLNOTLAR
                        join d2 in db.TBLOGENCİ
                        on d1.OGR equals d2.ID
                        join d3 in db.TBLDERSLER
                        on d1.DERS equals d3.DERSID


                        select new
                        {
                            ÖĞRENCİ = d2.AD + " " + d2.SOYAD,
                            DERS = d3.DERSAD,
                            SINAV1 = d1.SINAV1,
                            SINAV2 = d1.SINAV2,
                            SINAV3 = d1.SINAV3,
                            ORTALAMA = d1.ORTALAMA
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}
