using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace Numara_Sorgulama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string kaynakKod = null;
        string kaynakKod2 = null;
        int durum = 0;


        #region FORM_LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
           webBrowser2.Navigate(textBox4.Text);

        }
        #endregion

        #region Manuel Sorgu
        #region Sorgu gönderme
        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "Bekleniyor..";
            label6.ForeColor = Color.Black;

            try
            {
                #region Operatör sorgulama
                string numara = textBox1.Text;
                numara = numara.Remove(0, 1);
                webBrowser2.Document.GetElementById("txtMsisdn").InnerText = numara;
                HtmlElementCollection elc2 = this.webBrowser2.Document.GetElementsByTagName("input");
                foreach (HtmlElement el2 in elc2)
                {
                    if (el2.GetAttribute("value").Equals("Sorgula"))
                    {
                        el2.InvokeMember("Click");
                    }
                }
                #endregion
            }
            catch { }
        }
        #endregion

        #region Yeni Sorgu
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser2.Navigate(textBox4.Text);
                textBox1.Text = "0";
                label6.Text = "Bekleniyor..";
                label6.ForeColor = Color.Black;
            }
            catch { }
        }
        #endregion

        #region Kontrol İşlemleri
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
              #region Operatör sorgulama
                if (kaynakKod2.IndexOf("hizmet aldığı işletmeci") != -1)
                {
                    label6.ForeColor = Color.DarkBlue;
                    string gelen = kaynakKod2;
                    int titleIndexBaslangici = gelen.IndexOf(textBox6.Text) + textBox6.TextLength;
                    int titleIndexBitisi = gelen.Substring(titleIndexBaslangici).IndexOf("</DIV>");
                    string cikti = gelen.Substring(titleIndexBaslangici, titleIndexBitisi).Remove(0, 11).Replace("numarası", "Numara");
                    label6.Text = cikti.Replace("hizmet aldığı işletmeci: ","");
                }
                else
                {
                    label6.Text = "Bekleniyor..";
                    label6.ForeColor = Color.Black;
                }

                if (kaynakKod2.IndexOf("numarası bir işletmeciye ait değildir.") != -1)
                {
                    label6.Text = "Bu numara herhangi bir işletmeciye ait değildir";
                }
              #endregion
            }
            catch { }
        }
        #endregion

        #region Operatör sorgulama site kaynak kod
        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                kaynakKod2 = webBrowser2.Document.Body.InnerHtml.ToString(); // site kaynak kod
                textBox5.Text = kaynakKod2;
            }
            catch { }
        }
        #endregion
        #endregion

        #region Programı Kapat
        void kaydet()
        {
            try
            {

                Ayar ayar = new Ayar(Application.StartupPath + @"\ayar.ini");
                if (listBox1.Items.Count - 1 != listBox1.SelectedIndex)
                {
                    ayar.yaz("Genel Ayalar", "NumaraYol", textBox13.Text);
                    ayar.yaz("Genel Ayalar", "NumaraIndex", textBox14.Text);

                    DateTime dt = DateTime.Now;
                    string kayitYol = Application.StartupPath + @"\Kayıtlar\Sorgulanan-" + String.Format("{0:M/d/yyyy}", dt) + ".txt";
                    using (StreamWriter sw = new StreamWriter(kayitYol))
                    {
                        if (listView1.Items.Count > 0) // listview boş değil ise 
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (ColumnHeader baslik in listView1.Columns) // columns
                            {
                                sb.Append(string.Format("{0}\t", baslik.Text));
                            }
                            sw.WriteLine(sb.ToString());
                            foreach (ListViewItem lvi in listView1.Items)
                            {
                                sb = new StringBuilder();
                                foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
                                {
                                    sb.Append(string.Format("{0}\t", listViewSubItem.Text));
                                }
                                sw.WriteLine(sb.ToString());
                            }
                            sw.WriteLine();
                        }
                    }
                    ayar.yaz("Genel Ayalar", "SorgulananlarYol", kayitYol);
                }
                else
                {
                    if (timer2.Enabled == false)
                    {

                        ayar.yaz("Genel Ayalar", "NumaraYol", textBox13.Text);
                        ayar.yaz("Genel Ayalar", "NumaraIndex", textBox14.Text);

                        DateTime dt = DateTime.Now;
                        string kayitYol = Application.StartupPath + @"\Kayıtlar\Sorgulanan-" + String.Format("{0:M/d/yyyy}", dt) + ".txt";
                        using (StreamWriter sw = new StreamWriter(kayitYol))
                        {
                            if (listView1.Items.Count > 0) // listview boş değil ise 
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (ColumnHeader baslik in listView1.Columns) // columns
                                {
                                    sb.Append(string.Format("{0}\t", baslik.Text));
                                }
                                sw.WriteLine(sb.ToString());
                                foreach (ListViewItem lvi in listView1.Items)
                                {
                                    sb = new StringBuilder();
                                    foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
                                    {
                                        sb.Append(string.Format("{0}\t", listViewSubItem.Text));
                                    }
                                    sw.WriteLine(sb.ToString());
                                }
                                sw.WriteLine();
                            }
                        }
                        ayar.yaz("Genel Ayalar", "SorgulananlarYol", kayitYol);
                    }
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Proxy.VarsayılanProxy();
            }
            if (durum == 1)
            {
                kaydet();
            }
            Application.Exit();
        }

        private void programıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Proxy.VarsayılanProxy();
            }
            if (durum == 1)
            {
                kaydet();
            }
            Application.Exit();
        }

        private void programHakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Proxy.VarsayılanProxy();
            }
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Proxy.VarsayılanProxy();
            }
            kaydet();
            Application.Exit();
        }
        #endregion

        #region Otomatik Sorgu

        #region Numara Listesi Aktarma
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count > 1)
                {
                    lbl_Durum.ForeColor = Color.Blue;
                    lbl_Durum.Text = "Numara listesinde numaralar mevcut.";

                    DialogResult soru = MessageBox.Show("Numara listesinde numaralar mevcut.\nSilmek istiyorsanız 'Evet' butonuna tıklayınız..\nİstemiyorsanız otomatik tarama başlatabilirsiniz.", "Bildirim", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (soru == DialogResult.Yes)
                    {
                        listBox1.Items.Clear();
                        textBox2.Clear();
                    }
                }
                else
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.FileName = "";
                    open.Filter = "Txt Dosyası|*.txt";
                    DialogResult dosya = open.ShowDialog();
                    if (dosya == DialogResult.OK)
                    {
                        textBox13.Text = open.FileName;
                        StreamReader oku;
                        oku = File.OpenText(open.FileName);
                        string yaz;
                        while ((yaz = oku.ReadLine()) != null)
                        {
                            yaz = yaz.Replace("+90", "");
                            yaz = yaz.Replace("90", "");
                            string varmı = yaz.Substring(0, 1);
                            if (varmı != "0")
                            {
                                yaz = 0 + yaz;
                            }
                            yaz = yaz.Remove(0, 1);
                            listBox1.Items.Add(yaz.ToString());
                        }
                        oku.Close();

                        lbl_Durum.ForeColor = Color.DarkGreen;
                        lbl_Durum.Text = listBox1.Items.Count.ToString() + " adet numara yüklendi..";
                        textBox2.Text = listBox1.Items.Count.ToString();
                    }
                    else
                    {
                        lbl_Durum.ForeColor = Color.Blue;
                        lbl_Durum.Text = "Numara yüklemeyi iptal ettiniz.";
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Başlat
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Ayar ayar = new Ayar(Application.StartupPath + @"\ayar.ini");
                if (ayar.oku("Genel Ayalar", "NumaraYol") != "0")
                {
                    DialogResult soru = MessageBox.Show("Eski tarama verileri mevcut devam etmek ister misiniz?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (soru == DialogResult.No)
                    {
                        if (listBox1.Items.Count > 1)
                        {
                            textBox3.Clear();
                            textBox7.Clear();
                            textBox8.Clear();
                            textBox9.Clear();
                            listView1.Items.Clear();

                            lbl_Durum.ForeColor = Color.DarkGreen;
                            lbl_Durum.Text = "Otomatik numara sorgulama başladı...";

                            button3.Enabled = false;
                            button4.Enabled = false;
                            listBox1.Enabled = false;
                            listBox1.SelectedIndex = -1;
                            button5.Enabled = true;
                            timer2.Enabled = true;
                            gecenSure = 0;

                        }
                        else
                        {
                            MessageBox.Show("Numara listesi boş taramayı başlatabilmek için numara eklemeniz gerek!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (soru == DialogResult.Yes)
                    {
                        lbl_Durum.ForeColor = Color.DarkGreen;
                        lbl_Durum.Text = "Otomatik numara sorgulama başladı...";

                        button3.Enabled = false;
                        button4.Enabled = false;
                        listBox1.Enabled = false;

                        textBox13.Text = ayar.oku("Genel Ayalar", "NumaraYol");

                        StreamReader oku;
                        oku = File.OpenText(ayar.oku("Genel Ayalar", "NumaraYol"));
                        string yaz;
                        while ((yaz = oku.ReadLine()) != null)
                        {
                            yaz = yaz.Replace("+90", "");
                            yaz = yaz.Replace("90", "");
                            string varmı = yaz.Substring(0, 1);
                            if (varmı != "0")
                            {
                                yaz = 0 + yaz;
                            }
                            yaz = yaz.Remove(0, 1);
                            listBox1.Items.Add(yaz.ToString());
                        }
                        oku.Close();

                        



                        lbl_Durum.ForeColor = Color.DarkGreen;
                        lbl_Durum.Text = listBox1.Items.Count.ToString() + " adet numara yüklendi..";
                        textBox2.Text = listBox1.Items.Count.ToString();
                        listBox1.SelectedIndex = int.Parse(ayar.oku("Genel Ayalar", "NumaraIndex"));

                        button5.Enabled = true;
                        timer2.Enabled = true;
                        gecenSure = 0;
                    }
                }
                else
                {
                    if (listBox1.Items.Count > 1)
                    {
                        if (listView1.Items.Count > 1)
                        {
                            DialogResult soru = MessageBox.Show("Sorgulanan numaralar mevcut.\nYeni tarama başlatmak istiyorsanuz 'Evet' butonuna tıklayınız.\nİstemiyorsanuz 'Hayır' butonuna tıklayarak taramayı kaldığı yerdem devam ettirebilirsiniz.", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (soru == DialogResult.Yes)
                            {
                                textBox3.Clear();
                                textBox7.Clear();
                                textBox8.Clear();
                                textBox9.Clear();
                                listView1.Items.Clear();

                                lbl_Durum.ForeColor = Color.DarkGreen;
                                lbl_Durum.Text = "Otomatik numara sorgulama başladı...";

                                button3.Enabled = false;
                                button4.Enabled = false;
                                listBox1.Enabled = false;
                                listBox1.SelectedIndex = -1;
                                button5.Enabled = true;
                                timer2.Enabled = true;
                                gecenSure = 0;
                            }
                            else if (soru == DialogResult.No)
                            {
                                lbl_Durum.ForeColor = Color.DarkGreen;
                                lbl_Durum.Text = "Otomatik numara sorgulama başladı...";

                                button3.Enabled = false;
                                button4.Enabled = false;
                                listBox1.Enabled = false;

                                StreamReader oku;
                                oku = File.OpenText(ayar.oku("Genel Ayalar", "NumaraYol"));
                                string yaz;
                                while ((yaz = oku.ReadLine()) != null)
                                {
                                    yaz = yaz.Replace("+90", "");
                                    yaz = yaz.Replace("90", "");
                                    string varmı = yaz.Substring(0, 1);
                                    if (varmı != "0")
                                    {
                                        yaz = 0 + yaz;
                                    }
                                    yaz = yaz.Remove(0, 1);
                                    listBox1.Items.Add(yaz.ToString());
                                }
                                oku.Close();
                                lbl_Durum.ForeColor = Color.DarkGreen;
                                lbl_Durum.Text = listBox1.Items.Count.ToString() + " adet numara yüklendi..";
                                textBox2.Text = listBox1.Items.Count.ToString();
                                listBox1.SelectedIndex = int.Parse(ayar.oku("Genel Ayalar", "NumaraIndex"));

                                button5.Enabled = true;
                                timer2.Enabled = true;
                                gecenSure = 0;
                            }
                        }
                        else
                        {
                            lbl_Durum.ForeColor = Color.DarkGreen;
                            lbl_Durum.Text = "Otomatik numara sorgulama başladı...";

                            button3.Enabled = false;
                            button4.Enabled = false;
                            listBox1.Enabled = false;
                            listBox1.SelectedIndex = -1;
                            button5.Enabled = true;
                            timer2.Enabled = true;
                            gecenSure = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Numara listesi boş taramayı başlatabilmek için numara eklemeniz gerek!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Durdur
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Devam eden taramayı durdurmak istediniz.\nDaha sonra devam etmek istiyorsanız tarama verilerini 'Evet' butonuna tıklayarak kaydedebilirsiniz.","Bilgi",MessageBoxButtons.YesNo,MessageBoxIcon.Information);

            if (soru == DialogResult.Yes) 
            {
                kaydet();

                durum = 1;
                lbl_Durum.ForeColor = Color.DarkRed;
                lbl_Durum.Text = "Otomatik numara sorgulama durduruldu ve tarama verileri kaydedildi..";

                button5.Enabled = false;
                timer2.Enabled = false;
                limit = 0;
                webBrowser1.Stop();
                Proxy.VarsayılanProxy();

                button3.Enabled = true;
                button4.Enabled = true;
                listBox1.Enabled = true;
                listBox1.SelectedIndex = -1;
            }
            else
            {

                lbl_Durum.ForeColor = Color.DarkRed;
                lbl_Durum.Text = "Otomatik numara sorgulama durduruldu...";

                button5.Enabled = false;
                timer2.Enabled = false;
                limit = 0;
                webBrowser1.Stop();
                Proxy.VarsayılanProxy();

                button3.Enabled = true;
                button4.Enabled = true;
                listBox1.Enabled = true;
                listBox1.SelectedIndex = -1;
            }
        }
        #endregion

        #region İşlemler
        int limit = 0;
        int taranan = 0;
        int gecenSure = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count - 1 == listBox1.SelectedIndex)
                {
                    lbl_Durum.ForeColor = Color.DarkGreen;
                    lbl_Durum.Text = "Otomatik numara sorgulama tamamlandı. Taranan numara [" + listBox1.Items.Count.ToString() + "]";

                    button5.Enabled = false;
                    timer2.Enabled = false;
                    limit = 0;
                    webBrowser1.Stop();
                    if (checkBox1.Checked == true)
                    {
                        Proxy.VarsayılanProxy();
                    }
                    button3.Enabled = true;
                    button4.Enabled = true;
                    listBox1.Enabled = true;
                    listBox1.SelectedIndex = -1;
                }
                else
                {
                    limit = limit + 1;
                    label14.Text = limit.ToString();


                    gecenSure = gecenSure + 1;
                    label16.Text = "Geçen süre: " + gecenSure.ToString() + " sn";

                    if (limit == int.Parse(textBox10.Text))
                    {
                        if (checkBox1.Checked == true)
                        {
                            Random rnd = new Random();
                            listBox_Proxy.SelectedIndex = rnd.Next(0, listBox_Proxy.Items.Count);
                            Proxy.ProxyAyarla(listBox_Proxy.Text);
                        }

                        webBrowser1.Navigate(textBox4.Text);

                        listBox1.SelectedIndex = listBox1.SelectedIndex + 1;

                        textBox14.Text = listBox1.SelectedIndex.ToString();

                        lbl_Durum.ForeColor = Color.DarkGreen;
                        lbl_Durum.Text = listBox1.Text + " numarası sorgulanıyor..";

                        textBox3.Text = listBox1.Text;


                        if (textBox3.Text.Length == 10)
                        {
                            //webBrowser1.Navigate(textBox4.Text);
                        }
                        else
                        {
                            lbl_Durum.ForeColor = Color.DarkRed;
                            lbl_Durum.Text = listBox1.Text + " geçersiz numara diğer numaraya atlandı.";
                            limit = 0;
                        }

                        taranan = taranan + 1;
                        textBox8.Text = taranan.ToString();
                        int kalan = listBox1.Items.Count - taranan;
                        textBox9.Text = kalan.ToString();
                    }

                    if (limit == 1)
                    {
                        proxyList();
                    }

                    if (limit == int.Parse(textBox12.Text))
                    {
                        try
                        {
                            listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
                            limit = 0;
                            taranan = taranan - 1;
                            textBox8.Text = taranan.ToString();
                            int kalan = listBox1.Items.Count - taranan;
                            textBox9.Text = kalan.ToString();
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Operator Yakalama
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                kaynakKod = webBrowser1.Document.Body.InnerHtml.ToString();

                if (webBrowser1.StatusText == "Bitti" || webBrowser1.StatusText == "Done")
                {
                    if (kaynakKod.IndexOf("Güvenlik Resmi") != -1)
                    {
                        lbl_Durum.ForeColor = Color.DarkRed;
                        lbl_Durum.Text = listBox1.Text + " numarası güvenlik koduna takıldı..Proxy değiştiriliyor..";
                    }
                    else
                    {
                        #region Operatör sorgulama
                        try
                        {
                            #region Operatör sorgulama
                            webBrowser1.Document.GetElementById("txtMsisdn").InnerText = textBox3.Text;
                            HtmlElementCollection elc2 = this.webBrowser1.Document.GetElementsByTagName("input");
                            foreach (HtmlElement el2 in elc2)
                            {
                                if (el2.GetAttribute("value").Equals("Sorgula"))
                                {
                                    el2.InvokeMember("Click");
                                }
                            }
                            #endregion
                        }
                        catch { }

                        if (kaynakKod.IndexOf("reminderContainer") != -1)
                        {
                            try
                            {
                                #region Operatör sorgulama
                                if (kaynakKod.IndexOf("hizmet aldığı işletmeci") != -1)
                                {
                                    string gelen = kaynakKod;
                                    int titleIndexBaslangici = gelen.IndexOf(textBox6.Text) + textBox6.TextLength;
                                    int titleIndexBitisi = gelen.Substring(titleIndexBaslangici).IndexOf("</DIV>");
                                    string cikti = gelen.Substring(titleIndexBaslangici, titleIndexBitisi).Remove(0, 11).Replace("numarası", "Numara");
                                    textBox7.Text = cikti.Replace("hizmet aldığı işletmeci: ", "");
                                    lbl_Durum.ForeColor = Color.DarkGreen;
                                    lbl_Durum.Text = listBox1.Text + " - " + textBox7.Text;
                                }
                                else
                                {
                                    lbl_Durum.ForeColor = Color.DarkGreen;
                                    lbl_Durum.Text = listBox1.Text + " numarası bilinmiyor..";
                                    textBox7.Text = "Bilinmiyor";
                                }

                                if (kaynakKod.IndexOf("numarası bir işletmeciye ait değildir.") != -1)
                                {
                                    textBox7.Text = "Bilinmiyor";
                                }
                                #endregion

                                string[] veri = { textBox3.Text, textBox7.Text };
                                ListViewItem numara = new ListViewItem(veri);

                                listView1.Items.Add(numara);

                                limit = 0;
                            }
                            catch { }
                        }
                        #endregion
                    }
                }
                else
                {

                    lbl_Durum.ForeColor = Color.DarkRed;
                    lbl_Durum.Text = "Kaynak siteye erişilemiyor.. İnternet bağlantınızı kontrol edin ve taramayı yeniden başlatın..";
                    button5.Enabled = false;
                    timer2.Enabled = false;
                    limit = 0;
                    webBrowser1.Stop();

                    button3.Enabled = true;
                    button4.Enabled = true;
                    listBox1.Enabled = true;
                    listBox1.SelectedIndex = -1;
                    MessageBox.Show("Kaynak siteye erişilemiyor.. İnternet bağlantınızı kontrol edin ve taramayı yeniden başlatın..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch { }
        }
        #endregion

        #region Sonuçları Kaydet
        private void sonuçlarıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "";
                save.Filter = "Txt Dosyası|*.txt";

                DialogResult soru = save.ShowDialog();

                if (soru == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(save.FileName))
                    {
                        if (listView1.Items.Count > 0) // listview boş değil ise 
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (ColumnHeader baslik in listView1.Columns) // columns
                            {
                                sb.Append(string.Format("{0}\t", baslik.Text));
                            }
                            sw.WriteLine(sb.ToString());
                            foreach (ListViewItem lvi in listView1.Items)
                            {
                                sb = new StringBuilder();
                                foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
                                {
                                    sb.Append(string.Format("{0}\t", listViewSubItem.Text));
                                }
                                sw.WriteLine(sb.ToString());
                            }
                            sw.WriteLine();

                            lbl_Durum.ForeColor = Color.DarkGreen;
                            lbl_Durum.Text = "Sonuçlar başarıyla kaydedildi.";

                            MessageBox.Show("Sonuçlar Başarıyla Kaydedildi..", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    lbl_Durum.ForeColor = Color.Blue;
                    lbl_Durum.Text = "Sonuçları kaydetmeyi iptal ettiniz.";
                }
            }
            catch { }
        }
        #endregion

        #region Proxy List Çek
        private void proxyList()
        {
            try
            {
                listBox_Proxy.Items.Clear();

                Uri url = new Uri("http://proxy-list.org/english/index.php?p=1");
                WebClient client = new WebClient() { Encoding = Encoding.UTF8 };
                string html = client.DownloadString(url);
                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);

                HtmlNodeCollection XPath = dokuman.DocumentNode.SelectNodes("//*[@id='proxy-table']/div[2]/div");
                foreach (var veri in XPath)
                {
                    richTextBox1.Text = veri.InnerHtml;
                }

                HtmlAgilityPack.HtmlDocument dokuman2 = new HtmlAgilityPack.HtmlDocument();
                dokuman2.LoadHtml(richTextBox1.Text);
                HtmlNodeCollection XPath2 = dokuman2.DocumentNode.SelectNodes("//li[@class='proxy']");
                foreach (var veri2 in XPath2)
                {
                    listBox_Proxy.Items.Add(veri2.InnerText);
                }
                textBox11.Text = listBox_Proxy.Items.Count.ToString();
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.FileName = "";
                open.Filter = "Txt Dosyası|*.txt";
                DialogResult dosya = open.ShowDialog();
                if (dosya == DialogResult.OK)
                {
                    StreamReader oku;
                    oku = File.OpenText(open.FileName);
                    string yaz;
                    while ((yaz = oku.ReadLine()) != null)
                    {
                        listBox_Proxy.Items.Add(yaz.ToString());
                    }
                    oku.Close();

                    lbl_Durum.ForeColor = Color.DarkGreen;
                    lbl_Durum.Text = listBox_Proxy.Items.Count.ToString() + " adet proxy yüklendi..";
                    textBox11.Text = listBox_Proxy.Items.Count.ToString();
                }
                else
                {
                    lbl_Durum.ForeColor = Color.Blue;
                    lbl_Durum.Text = "Proxy list yüklemeyi iptal ettiniz.";
                }
            }
            catch { }
        }
        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Eski tarama verileri silinsin mi?","Bildirim",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (soru == DialogResult.Yes)
            {
                Ayar ayar = new Ayar(Application.StartupPath + @"\ayar.ini");
                ayar.yaz("Genel Ayalar", "NumaraYol", "0");
                ayar.yaz("Genel Ayalar", "NumaraIndex", "0");
                ayar.yaz("Genel Ayalar", "SorgulananlarYol", "0");

                lbl_Durum.ForeColor = Color.DarkGreen;
                lbl_Durum.Text = "Tarama verileri başarıyla silindi.";
                MessageBox.Show("Tarama verileri başarıyla silindi.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        #endregion
    }
}
