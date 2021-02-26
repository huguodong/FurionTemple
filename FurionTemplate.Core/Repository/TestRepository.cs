using Furion.DependencyInjection;
using FurionTemplate.Core.Enity;
using SqlSugar;

namespace FurionTemplate.Core.Repository
{
    public class TestRepository : BaseRepository<TestTable>, ITestRepository, ITransient
    {
        public TestRepository(ISqlSugarRepository<TestTable> sqlSugarRepository) : base(sqlSugarRepository)
        {
        }
    }
}
