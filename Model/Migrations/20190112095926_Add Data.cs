using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuestionPurposes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Свойство" });

            migrationBuilder.InsertData(
                table: "QuestionPurposes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Оценка клиента" });

            migrationBuilder.InsertData(
                table: "QuestionPurposes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Сегмент" });

            migrationBuilder.InsertData(
                table: "QuestionPurposes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Неиспользуемое" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Вещественный" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Строковый" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Дата/Время" });

            migrationBuilder.InsertData(
                table: "QuestionViews",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Непрерывный" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuestionPurposes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuestionPurposes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuestionPurposes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "QuestionPurposes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "QuestionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuestionTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuestionTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "QuestionViews",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
