using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace veritabani_sifreli_veri_yukleme_cekme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection baglanti = new MySqlConnection("Server=localhost;Database=sifreli_veriler;Uid=root;Pwd=root;");
        void listele()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "ADI";
            dataGridView1.Columns[2].Name = "SOYADI";
            dataGridView1.Columns[3].Name = "MAİL";
            baglanti.Open();
            MySqlCommand cmd = new MySqlCommand("select kisi_id,kisi_ad,kisi_soyad,kisi_mail from kisiler", baglanti);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                byte[] adcozum = Convert.FromBase64String(dr[1].ToString());
                string ad = ASCIIEncoding.ASCII.GetString(adcozum);
                byte[] soyadcozum = Convert.FromBase64String(dr[2].ToString());
                string soyad = ASCIIEncoding.ASCII.GetString(soyadcozum);
                byte[] mailcozum = Convert.FromBase64String(dr[3].ToString());
                string mail = ASCIIEncoding.ASCII.GetString(mailcozum);
                string[] veriler = { dr[0].ToString(), ad, soyad, mail };
                dataGridView1.Rows.Add(veriler);
                Array.Clear(veriler, 0, 3);
            }
            baglanti.Close();
        }
        private void btn_ekle_Click_1(object sender, EventArgs e)
        {
            byte[] addizi = ASCIIEncoding.ASCII.GetBytes(txt_ad.Text);
            string adsifre = Convert.ToBase64String(addizi);

            byte[] soyaddizi = ASCIIEncoding.ASCII.GetBytes(txt_soyad.Text);
            string soyadsifre = Convert.ToBase64String(soyaddizi);

            byte[] maildizi = ASCIIEncoding.ASCII.GetBytes(txt_mail.Text);
            string mailsifre = Convert.ToBase64String(maildizi);

            baglanti.Open();
            MySqlCommand cmd = new MySqlCommand("insert into kisiler (kisi_ad,kisi_soyad,kisi_mail) values (@ad,@soyad,@mail)", baglanti);
            cmd.Parameters.AddWithValue("@ad", adsifre);
            cmd.Parameters.AddWithValue("@soyad", soyadsifre);
            cmd.Parameters.AddWithValue("@mail", mailsifre);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Veri Eklendi","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btn_listele_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            byte[] sifreliad = ASCIIEncoding.ASCII.GetBytes(txt_ad.Text);
            string ad = Convert.ToBase64String(sifreliad);
            byte[] sifrelisoyad = ASCIIEncoding.ASCII.GetBytes(txt_soyad.Text);
            string soyad = Convert.ToBase64String(sifrelisoyad);
            byte[] sifrelimail = ASCIIEncoding.ASCII.GetBytes(txt_mail.Text);
            string mail = Convert.ToBase64String(sifrelimail);
            baglanti.Open();
            MySqlCommand cmd2 = new MySqlCommand("update kisiler set kisi_ad=@ad,kisi_soyad=@soyad,kisi_mail=@mail where kisi_id=@id", baglanti);
            cmd2.Parameters.AddWithValue("@ad", ad);
            cmd2.Parameters.AddWithValue("@soyad", soyad);
            cmd2.Parameters.AddWithValue("@mail", mail);
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.ExecuteNonQuery();
            MessageBox.Show("Veriler Güncellendi","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
            baglanti.Close();
            listele();
        }
        string id;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_ad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_soyad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_mail.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            MySqlCommand cmd3 = new MySqlCommand("delete from kisiler where kisi_id=" + id, baglanti);
            cmd3.ExecuteNonQuery();
            MessageBox.Show("Veriler Silindi","Silme",MessageBoxButtons.OK,MessageBoxIcon.Information);
            baglanti.Close();
            listele();
        }
    }
}
