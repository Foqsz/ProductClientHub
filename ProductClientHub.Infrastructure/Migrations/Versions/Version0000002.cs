using FluentMigrator;

namespace ProductClientHub.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_PRODUCT, "Create table to save the product's information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Products")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Brand").AsString(255).NotNullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable()
            .WithColumn("ClientId").AsGuid().NotNullable();
    }
}
