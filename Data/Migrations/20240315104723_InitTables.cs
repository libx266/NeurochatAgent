using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prompt = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: false),
                    responce = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: false),
                    model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    completion_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    temperature = table.Column<float>(type: "real", nullable: false),
                    repetition_penalty = table.Column<float>(type: "real", nullable: false),
                    top_p = table.Column<float>(type: "real", nullable: false),
                    prompt_tokens = table.Column<int>(type: "integer", nullable: false),
                    completion_tokens = table.Column<int>(type: "integer", nullable: false),
                    external_id = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    api_service = table.Column<int>(type: "integer", nullable: false),
                    additional_info = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    external_id = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    api_service = table.Column<int>(type: "integer", nullable: false),
                    additional_info = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    photo_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: true),
                    data = table.Column<byte[]>(type: "bytea", maxLength: 16777216, nullable: true),
                    attach_type = table.Column<int>(type: "integer", nullable: false),
                    message_id = table.Column<int>(type: "integer", nullable: true),
                    LocalContent = table.Column<byte[]>(type: "bytea", nullable: false),
                    external_id = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    api_service = table.Column<int>(type: "integer", nullable: false),
                    additional_info = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_type = table.Column<int>(type: "integer", nullable: false),
                    external_id = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    api_service = table.Column<int>(type: "integer", nullable: false),
                    additional_info = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    photo_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_files_photo_id",
                        column: x => x.photo_id,
                        principalTable: "files",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    sender_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    message_type = table.Column<int>(type: "integer", nullable: false),
                    replay_message_id = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    api_service = table.Column<int>(type: "integer", nullable: false),
                    additional_info = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_messages_replay_message_id",
                        column: x => x.replay_message_id,
                        principalTable: "messages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chats_photo_id",
                table: "chats",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "IX_files_message_id",
                table: "files",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_replay_message_id",
                table: "messages",
                column: "replay_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_photo_id",
                table: "users",
                column: "photo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_chats_files_photo_id",
                table: "chats",
                column: "photo_id",
                principalTable: "files",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_files_messages_message_id",
                table: "files",
                column: "message_id",
                principalTable: "messages",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_files_photo_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "generations");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
