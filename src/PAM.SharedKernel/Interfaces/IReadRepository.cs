using Ardalis.Specification;

namespace PAM.SharedKernel.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
        IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false);

        IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification);
    }
}
