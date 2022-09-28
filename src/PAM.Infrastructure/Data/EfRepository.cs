using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using PAM.SharedKernel.Interfaces;

namespace PAM.Infrastructure.Data
{
    // inherit from Ardalis.Specification type
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public new IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return base.ApplySpecification(specification, evaluateCriteriaOnly);
        }

        public new IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
          return base.ApplySpecification(specification);
        }
    
    }
}
