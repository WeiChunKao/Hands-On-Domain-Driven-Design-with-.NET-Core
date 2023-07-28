using Marketplace.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure
{
    /// <summary>
    /// �~��DbContext�A�N��F���ε{������Ʈw�W�U��C
    /// DbContext �O Entity Framework Core ���֤����O�A�Ω�P��Ʈw�i��椬�C
    /// </summary>
    public class ClassifiedAdDbContext : DbContext
    {
        /// <summary>
        /// ILoggerFactory�O DbContext ����x�u�t�A�i�H�l�� DbContext ���ާ@�C
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// �z�L DbContextOptions<ClassifiedAdDbContext> options �M ILoggerFactory loggerFactory �Ѽƶi���l�ơC
        /// DbContextOptions �]�t�F�����Ʈw�s�u�M��L DbContext �ﶵ����T�C
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public ClassifiedAdDbContext(
            DbContextOptions<ClassifiedAdDbContext> options,
            ILoggerFactory loggerFactory)
            : base(options)
            => _loggerFactory = loggerFactory;

        /// <summary>
        /// ��Ʈw�W�U�媺�ݩʡA�N��F ClassifiedAd ���O����ƪ�C
        /// �z�L DbContext �i�H��o�Ӹ�ƪ�i�� CRUD �ާ@�C
        /// </summary>
        public DbSet<ClassifiedAd> ClassifiedAds { get; set; }

        /// <summary>
        /// �O DbContext ����k�A�|�b DbContext �Q�ЫخɽեΡA�Ω�i�� DbContext ������]�w�C
        /// �b�o�̳]�w�F DbContext ����x�u�t�M�ҥΤF�ӷP�ƾڤ�x�]sensitive data logging�^�C
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        ///  �O DbContext ����k�A�Ω�i����髬�O�M��Ʈw��椧�����M�g�]�w�]mapping configuration�^�C
        ///  �ϥ� ClassifiedAdEntityTypeConfiguration �M PictureEntityTypeConfiguration ���O�i�� ClassifiedAd �M Picture ���骺�M�g�]�w�C
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassifiedAdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
        }
    }

    /// <summary>
    /// ��@�F IEntityTypeConfiguration<ClassifiedAd> �����O�A�Ω�t�m ClassifiedAd ���O������P��Ʈw��檺�M�g�]�w�C
    /// </summary>
    public class ClassifiedAdEntityTypeConfiguration : IEntityTypeConfiguration<ClassifiedAd>
    {
        /// <summary>
        /// �ϥ� builder �� ClassifiedAd ���O���ݩʶi��F�t�m�A�]�A�D��B�֦��������ݩʡ]OwnsOne�^���C
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
    /// ��@�F IEntityTypeConfiguration<Picture> �����O�A�Ω�t�m Picture ���O������P��Ʈw��檺�M�g�]�w�C
    /// </summary>
    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
    {
        /// <summary>
        /// �ϥ� builder �� Picture ���O���ݩʶi��F�t�m�A�]�A�D��B�֦��������ݩʡ]OwnsOne�^���C
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
    /// �o�O�@���X�R��k�]extension method�^�A�w�q�F EnsureDatabase ��k�A�Ω�T�O��Ʈw�w�Ыبöi��E���]migration�^�C
    /// </summary>
    public static class AppBuilderDatabaseExtensions
    {
        /// <summary>
        /// app.ApplicationServices.GetService<ClassifiedAdDbContext>() ��������ε{���� ClassifiedAdDbContext ��ҡA�ϥ� EnsureCreated() ��k�T�O��Ʈw�w�ЫءC
        /// �p�G��Ʈw�w�g�s�b�A�h�ϥ� Migrate() ��k�i���Ʈw�E���A�T�O��Ʈw�����c�M�ҫ��O���P�B�C
        /// �b���ε{���ҰʮɡA��Ʈw�N�|�۰ʳЫةζi��E���A�H�T�O��Ʈw�����T���A�C
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