namespace ProcesarArchivoMudanzas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogMudanzas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Documento = c.Int(nullable: false),
                        FechaProceso = c.DateTime(nullable: false),
                        NumeroViajes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogMudanzas");
        }
    }
}
