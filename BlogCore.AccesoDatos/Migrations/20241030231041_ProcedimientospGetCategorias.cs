using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class ProcedimientospGetCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedimiento = @"CREATE PROCEDURE spGetCategorias 
                                   AS
                                   BEGIN
                                       SELECT * FROM Categoria
                                   END";
            
            migrationBuilder.Sql(procedimiento);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedimiento = @"DROP PROCEDURE spGetCategorias";
            
            migrationBuilder.Sql(procedimiento);
        }
    }
}
