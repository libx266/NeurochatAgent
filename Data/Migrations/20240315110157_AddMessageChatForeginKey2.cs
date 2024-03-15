using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageChatForeginKey2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_messages_chat_id",
                table: "messages",
                column: "chat_id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chats_chat_id",
                table: "messages",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_chats_chat_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_chat_id",
                table: "messages");
        }
    }
}
