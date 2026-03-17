using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace ProductClientHub.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("CreatedOn").AsDateTime().NotNullable();
    }
}
