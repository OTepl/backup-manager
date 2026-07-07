using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektTepZalohovac
{
    public class JsonBackupJobFileConfig
    {
        public List<string> Sources { get; set; }
        public List<string> Targets { get; set; }
        public string Method { get; set; }
        public string Timing { get; set; }
        public Retention Retention { get; set; }
    }

    public class Retention
    {
        public int Count { get; set; }
        public int Size { get; set; }
    }
}





































//listy pro vstup a vystup
// metoda cteni podle posledniho otevreni