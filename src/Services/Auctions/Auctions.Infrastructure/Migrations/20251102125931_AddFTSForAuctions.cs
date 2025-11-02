using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Auctions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFTSForAuctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:unaccent", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.Sql(
                sql: "CREATE TEXT SEARCH CONFIGURATION public.polish (COPY = simple);");
            
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Auctions",
                type: "tsvector",
                nullable: true);

            migrationBuilder.CreateIndex(
                    name: "IX_Auctions_SearchVector",
                    table: "Auctions",
                    column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.Sql(
                sql: @"
        CREATE OR REPLACE FUNCTION update_auction_search_vector()
        RETURNS TRIGGER AS $$
        BEGIN
            NEW.""SearchVector"" :=
                to_tsvector('polish', unaccent(COALESCE(NEW.""Title"", ''))) ||
                to_tsvector('english', unaccent(COALESCE(NEW.""Title"", ''))) ||
                to_tsvector('polish', unaccent(COALESCE(NEW.""Description"", ''))) ||
                to_tsvector('english', unaccent(COALESCE(NEW.""Description"", '')));
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
        ");

            migrationBuilder.Sql(
                sql: @"
        CREATE TRIGGER trg_auction_search_vector_update
        BEFORE INSERT OR UPDATE ON ""Auctions""
        FOR EACH ROW
        EXECUTE FUNCTION update_auction_search_vector();
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_auction_search_vector_update ON ""Auctions"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS update_auction_search_vector();");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_SearchVector",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Auctions");
        
            migrationBuilder.Sql(@"DROP TEXT SEARCH CONFIGURATION IF EXISTS public.polish;");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:ltree", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:unaccent", ",,");
        }
    }
}
