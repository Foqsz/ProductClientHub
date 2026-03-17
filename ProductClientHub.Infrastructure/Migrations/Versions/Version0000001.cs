using FluentMigrator;

namespace ProductClientHub.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_CLIENT, "Create table to save the client's information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}
