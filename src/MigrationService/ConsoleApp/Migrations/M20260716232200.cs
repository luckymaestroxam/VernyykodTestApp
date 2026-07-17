using FluentMigrator;

namespace MigrationService.ConsoleApp.Migrations;

[Migration(20260716232200, "Увеличение точности курса валюты")]
public class M20260716232200 : Migration
{
    public override void Up() =>
        Alter.Table("currency")
            .AlterColumn("rate").AsDecimal(18, 10).NotNullable();

    public override void Down() =>
        Alter.Table("currency")
            .AlterColumn("rate").AsDecimal().NotNullable();
}
