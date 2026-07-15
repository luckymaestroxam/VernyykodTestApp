using FluentMigrator;

namespace ConsoleApp.Migrations;

[Migration(20260715173500, "Добавление таблиц currency, user, user_currency")]
public class M20260715173500 : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("currency")
            .WithColumn("id").AsString().Unique().NotNullable()
            .WithColumn("name").AsString().Unique().NotNullable()
            .WithColumn("rate").AsDecimal().NotNullable();

        Create.Table("user")
            .WithColumn("id").AsGuid().Unique().NotNullable()
            .WithColumn("name").AsString().Unique().NotNullable()
            .WithColumn("password").AsString().NotNullable();

        Create.Table("user_currency")
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("currency_id").AsString().NotNullable();

        Create.ForeignKey("FK_userCurrency_userId")
            .FromTable("user_currency").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");

        Create.ForeignKey("FK_userCurrency_currencyId")
            .FromTable("user_currency").ForeignColumn("currency_id")
            .ToTable("currency").PrimaryColumn("id");

        Create.Index("IX_userCurrency_userId_currencyId")
            .OnTable("user_currency")
            .OnColumn("user_id").Ascending()
            .OnColumn("currency_id").Ascending()
            .WithOptions().Unique();
    }
}
