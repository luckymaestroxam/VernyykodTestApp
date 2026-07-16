using FluentMigrator;

namespace ConsoleApp.Migrations;

[Migration(20260716111300, "Добавление таблицы revoked_token")]
public class M20260716111300 : AutoReversingMigration
{
    public override void Up() =>
        Create.Table("revoked_token")
            .WithColumn("jti").AsGuid().Unique().NotNullable()
            .WithColumn("revoked_at").AsDateTime2().NotNullable();
}
