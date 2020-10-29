using GenericHostPractice.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GenericHostPractice.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestEntity> Tests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // ユニーク制約・インデックスが必要な場合は以下のように設定する
        //    modelBuilder.Entity<TestEntity>()
        //        .HasIndex(p => new { p.FirstName, p.LastName });
        //    base.OnModelCreating(modelBuilder);
        //}

        #region 標準項目の設定
        public int SaveChanges(string name)
        {
            SetBase(name);

            return base.SaveChanges();
        }

        public override int SaveChanges()
        {
            SetBase();

            return base.SaveChanges();
        }

        private void SetBase(string name = "System")
        {
            DateTime now = DateTime.UtcNow;

            // 追加エンティティのうち、IEntity を実装したものを抽出
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .OfType<EntityBase>();

            foreach (var entity in entities)
            {
                entity.UpdatedDate = now;
                entity.UpdatedBy = name;    // TODO:名前を入れる場合は、新しくメソッドを作る
                if (entity.CreatedDate is null)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = name;
                }
            }
        }
        #endregion
    }
}
