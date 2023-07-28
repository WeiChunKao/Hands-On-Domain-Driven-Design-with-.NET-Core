using System.Threading.Tasks;
using Marketplace.Framework;

namespace Marketplace.Infrastructure
{
    /// <summary>
    /// Unit of Work �O�@�ӼҦ��A�Ω�N�@�t�C����Ʈw�ާ@�]�Ҧp�s�W�B�ק�B�R���^�ʸ˦��@�ӳ�@������]Transaction�^�C
    /// �i�H�T�O�Ҧ��ާ@�n��������\����(Commit)�A�n��������Ѧ^�u(RollBack)�C
    /// </summary>
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// �O�e���w�q�� DbContext�A���O Entity Framework Core ���Ω�ާ@��Ʈw���D�n���O�C
        /// </summary>
        private readonly ClassifiedAdDbContext _dbContext;

        public EfCoreUnitOfWork(ClassifiedAdDbContext dbContext)
            => _dbContext = dbContext;

        /// <summary>
        /// Commit ��k�O IUnitOfWork ����������k�C
        /// Commit ��k�ϥ� _dbContext.SaveChangesAsync() �ӱN�Ҧ�����Ʈw�ާ@�x�s���Ʈw�C
        /// SaveChangesAsync �O DbContext �����@�ӫD�P�B��k�A�Ω󲧨B�x�s�Ҧ����Ʈw�����C�o�]�A�s�W�B�ק�M�R�����ާ@�C
        /// �^�ǫ��O�O Task�A�]���o�� Commit ��k�]�O�D�P�B���A���|�b��Ʈw�ާ@�������^�@�Ӫ�ܧ������A�� Task ����C
        /// </summary>
        /// <returns></returns>
        public Task Commit() => _dbContext.SaveChangesAsync();
    }
}