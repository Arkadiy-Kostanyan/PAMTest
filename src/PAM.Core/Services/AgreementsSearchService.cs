using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.Core.Interfaces;
using PAM.SharedKernel.Interfaces;

namespace PAM.Core.Services;

public record AgreementRecord(int Id, string UserName, string GroupCode, string ProductNumber, DateTime EffectiveDate, DateTime? ExpirationDate,
  decimal ProductPrice, decimal NewPrice, bool Active, string ProductDescription, string GroupDescription);

public class AgreementsSearchService : IAgreementsSearchService
{
  private readonly IReadRepository<Agreement> _repository;
  private readonly ICurrentUserService _currentUserService;
  private readonly UserManager<IdentityUser> _userManager;

  public AgreementsSearchService(IReadRepository<Agreement> repository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager)
  {
    _repository = repository;
    _currentUserService = currentUserService;
    _userManager = userManager;
  }
  public async Task<List<AgreementRecord>> GetAllAgreements()
  {
    var agreements = _repository.ApplySpecification(new AllAgreementsWithProductAndGroupSpec());

    return await (from agreement in agreements 
                   from user in _userManager.Users
                   where agreement.UserId == user.Id
                   select new AgreementRecord(
                     agreement.Id,
                     user.UserName,
                     agreement.Product.ProductGroup.GroupCode,
                     agreement.Product.ProductNumber,
                     agreement.EffectiveDate,
                     agreement.ExpirationDate,
                     agreement.ProductPrice,
                     agreement.NewPrice,
                     agreement.Active,
                     agreement.Product.Description,
                     agreement.Product.ProductGroup.Description
                   )).ToListAsync();
    
  }
}
