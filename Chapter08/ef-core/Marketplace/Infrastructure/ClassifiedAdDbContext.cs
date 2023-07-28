using Marketplace.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure
{
    /// <summary>
    /// 繼承DbContext，代表了應用程式的資料庫上下文。
    /// DbContext 是 Entity Framework Core 的核心類別，用於與資料庫進行交互。
    /// </summary>
    public class ClassifiedAdDbContext : DbContext
    {
        /// <summary>
        /// ILoggerFactory是 DbContext 的日誌工廠，可以追蹤 DbContext 的操作。
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// 透過 DbContextOptions<ClassifiedAdDbContext> options 和 ILoggerFactory loggerFactory 參數進行初始化。
        /// DbContextOptions 包含了關於資料庫連線和其他 DbContext 選項的資訊。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public ClassifiedAdDbContext(
            DbContextOptions<ClassifiedAdDbContext> options,
            ILoggerFactory loggerFactory)
            : base(options)
            => _loggerFactory = loggerFactory;

        /// <summary>
        /// 資料庫上下文的屬性，代表了 ClassifiedAd 類別的資料表。
        /// 透過 DbContext 可以對這個資料表進行 CRUD 操作。
        /// </summary>
        public DbSet<ClassifiedAd> ClassifiedAds { get; set; }

        /// <summary>
        /// 是 DbContext 的方法，會在 DbContext 被創建時調用，用於進行 DbContext 的全域設定。
        /// 在這裡設定了 DbContext 的日誌工廠和啟用了敏感數據日誌（sensitive data logging）。
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        ///  是 DbContext 的方法，用於進行實體型別和資料庫表格之間的映射設定（mapping configuration）。
        ///  使用 ClassifiedAdEntityTypeConfiguration 和 PictureEntityTypeConfiguration 類別進行 ClassifiedAd 和 Picture 實體的映射設定。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassifiedAdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
        }
    }

    /// <summary>
    /// 實作了 IEntityTypeConfiguration<ClassifiedAd> 的類別，用於配置 ClassifiedAd 類別的實體與資料庫表格的映射設定。
    /// </summary>
    public class ClassifiedAdEntityTypeConfiguration : IEntityTypeConfiguration<ClassifiedAd>
    {
        /// <summary>
        /// 使用 builder 對 ClassifiedAd 類別的屬性進行了配置，包括主鍵、擁有的實體屬性（OwnsOne）等。
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClassifiedAd> builder)
        {
            builder.HasKey(x => x.ClassifiedAdId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.Price, p => p.OwnsOne(c => c.Currency));
            builder.OwnsOne(x => x.Text);
            builder.OwnsOne(x => x.Title);
            builder.OwnsOne(x => x.ApprovedBy);
            builder.OwnsOne(x => x.OwnerId);
        }
    }

    /// <summary>
    /// 實作了 IEntityTypeConfiguration<Picture> 的類別，用於配置 Picture 類別的實體與資料庫表格的映射設定。
    /// </summary>
    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
    {
        /// <summary>
        /// 使用 builder 對 Picture 類別的屬性進行了配置，包括主鍵、擁有的實體屬性（OwnsOne）等。
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(x => x.PictureId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.ParentId);
            builder.OwnsOne(x => x.Size);
        }
    }

    /// <summary>
    /// 這是一個擴充方法（extension method），定義了 EnsureDatabase 方法，用於確保資料庫已創建並進行遷移（migration）。
    /// </summary>
    public static class AppBuilderDatabaseExtensions
    {
        /// <summary>
        /// app.ApplicationServices.GetService<ClassifiedAdDbContext>() 獲取到應用程式的 ClassifiedAdDbContext 實例，使用 EnsureCreated() 方法確保資料庫已創建。
        /// 如果資料庫已經存在，則使用 Migrate() 方法進行資料庫遷移，確保資料庫的結構和模型保持同步。
        /// 在應用程式啟動時，資料庫就會自動創建或進行遷移，以確保資料庫的正確狀態。
        /// </summary>
        /// <param name="app"></param>
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ClassifiedAdDbContext>();

            if (!context.Database.EnsureCreated())
                context.Database.Migrate();
        }
    }
}