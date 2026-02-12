using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySnakeCaseNamingConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Skip if mapped to a view or similar special cases if needed
            
            // Allow manual ToTable to take precedence (it's applied in configuration)
            // But if we wanted to auto-map Table names too, we could.
            // Given the specific prefixes (su_, db_), we will keep the manual ToTable in configurations for now 
            // OR we could try to detect them, but that's magic.
            // For now, I'll focus on COLUMN names which is the bulk of the boilerplate.

            foreach (var property in entity.GetProperties())
            {
                // Convert property name to snake_case
                var snakeCaseName = ToSnakeCase(property.Name);
                property.SetColumnName(snakeCaseName);
            }

            // Centralized Table Naming
            // Logic: Prefix = First 2 chars of ClassName (e.g. SuUser -> su, DbEmployee -> db)
            // Table Name = prefix + "_" + snake_case(Remainder)
            
            var className = entity.ClrType.Name;
            if (className.Length > 2)
            {
                var prefix = className.Substring(0, 2).ToLower();
                var remainder = className.Substring(2);
                var tableName = $"{prefix}_{ToSnakeCase(remainder)}";
                entity.SetTableName(tableName);
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()!));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()!));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()!));
            }

            // Custom Column Mapping for Audit Fields
            // Requested: createBy -> cr_by, createDate -> cr_date, updateBy -> upd_by, updateDate -> upd_date
            foreach (var property in entity.GetProperties())
            {
                switch (property.Name)
                {
                    case "CreateBy":
                        property.SetColumnName("cr_by");
                        break;
                    case "CreateDate":
                        property.SetColumnName("cr_date");
                        break;
                    case "UpdateBy":
                        property.SetColumnName("upd_by");
                        break;
                    case "UpdateDate":
                        property.SetColumnName("upd_date");
                        break;
                }
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
