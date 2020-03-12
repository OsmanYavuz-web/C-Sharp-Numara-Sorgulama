using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Numara_Sorgulama
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                webBrowser1.Document.GetElementById("kadi").InnerText = textBox1.Text;
                webBrowser1.Document.GetElementById("parola").InnerText = textBox2.Text;
                HtmlElementCollection elc2 = webBrowser1.Document.GetElementsByTagName("input");
                foreach (HtmlElement el2 in elc2)
                {
                    if (el2.GetAttribute("value").Equals("Giriş Yap"))
                    {
                        el2.InvokeMember("Click");
                    }
                }
            }
            catch 
            { 
            
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://kadinurunleri.com/program/");
            //webBrowser1.Navigate("http://localhost/");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string kaynakKod = webBrowser1.Document.Body.InnerHtml.ToString();

            button1.Enabled = true;

            if (kaynakKod.IndexOf("Gerekli alanları doldurunuz") != -1) 
            {
                MessageBox.Show("Gerekli alanları doldurunuz.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else if (kaynakKod.IndexOf("Başarılı") != -1)
            {
                MessageBox.Show("Giriş Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form1 frm1 = new Form1();
                frm1.Show();
            }
            else if (kaynakKod.IndexOf("Başarısız") != -1) 
            {
                MessageBox.Show("Giriş Başarısız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
