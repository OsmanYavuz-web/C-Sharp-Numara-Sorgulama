using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Numara_Sorgulama
{
    public class Ayar
    {
        public string kök;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string bölüm, string başlık, string değer, string dosyayolu);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string bölüm, string başlık, string bağıntı, StringBuilder düzen, int boyut, string dosyayolu);
        public Ayar(string inikök)
        {
            kök = inikök;
        }


        public void yaz(string bölüm, string başlık, string değer)
        {
            WritePrivateProfileString(bölüm, başlık, değer, this.kök);
        }

        public string oku(string bölüm, string başlık)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(bölüm, başlık, "", temp, 255, this.kök);
            return temp.ToString();
        }
    }
}
