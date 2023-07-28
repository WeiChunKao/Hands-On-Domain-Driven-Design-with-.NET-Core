using System.Threading.Tasks;
using Marketplace.Framework;

namespace Marketplace.Infrastructure
{
    /// <summary>
    /// Unit of Work 是一個模式，用於將一系列的資料庫操作（例如新增、修改、刪除）封裝成一個單一的交易（Transaction）。
    /// 可以確保所有操作要麼全部成功提交(Commit)，要麼全部失敗回滾(RollBack)。
    /// </summary>
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 是前面定義的 DbContext，它是 Entity Framework Core 中用於操作資料庫的主要類別。
        /// </summary>
        private readonly ClassifiedAdDbContext _dbContext;

        public EfCoreUnitOfWork(ClassifiedAdDbContext dbContext)
            => _dbContext = dbContext;

        /// <summary>
        /// Commit 方法是 IUnitOfWork 介面中的方法。
        /// Commit 方法使用 _dbContext.SaveChangesAsync() 來將所有的資料庫操作儲存到資料庫。
        /// SaveChangesAsync 是 DbContext 中的一個非同步方法，用於異步儲存所有對資料庫的更改。這包括新增、修改和刪除等操作。
        /// 回傳型別是 Task，因此這個 Commit 方法也是非同步的，它會在資料庫操作完成後返回一個表示完成狀態的 Task 物件。
        /// </summary>
        /// <returns></returns>
        public Task Commit() => _dbContext.SaveChangesAsync();
    }
}