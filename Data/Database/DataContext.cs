using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel = Data.Database.Entities.FileModel;

namespace Data.Database
{
    public sealed class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(System.IO.File.ReadAllLines(@"C:\Rozpodrawa\neuroagent.txt")[2]);
        }

        public DbSet<ChatModel> Chats { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<GenerationModel> Generations { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
