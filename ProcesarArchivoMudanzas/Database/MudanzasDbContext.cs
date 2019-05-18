using ProcesarArchivoMudanzas.Models;
using System;
using System.Data.Entity;

namespace ProcesarArchivoMudanzas.Database
{
    public partial class MudanzasDbContext : DbContext
    {
        public MudanzasDbContext():base("MudanzasContext")
        {

        }

        public virtual DbSet<LogMudanza> LogMudanza { get; set; }
      
    }
}