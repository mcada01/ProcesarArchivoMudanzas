using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcesarArchivoMudanzas.Models
{
    public class LogMudanza
    {
        public int Id { get; set; }
        public int Documento { get; set; }
        public DateTime ProcessDate { get; set; }
        public int CaseNumber { get; set; }
    }
}