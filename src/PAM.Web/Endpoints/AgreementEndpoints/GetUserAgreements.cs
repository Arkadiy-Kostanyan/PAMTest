using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.Core.AgreementAggregate;
using PAM.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using PAM.Core.Interfaces;
using PAM.Web.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class GetUserAgreements : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<AgreementResponse>
{
  private readonly IReadRepository<Agreement> _repository;
  private readonly ICurrentUserService _currentUserService;
  private readonly UserManager<IdentityUser> _userManager;

  public GetUserAgreements(IReadRepository<Agreement> repository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager)
  {
    _repository = repository;
    _currentUserService = currentUserService;
    _userManager = userManager;
  }

  [Authorize]
  [HttpGet("/api/v{version:apiVersion}/agreements")]
  [SwaggerOperation(
      Summary = "Gets a list of all current user Agreements",
      Description = "Gets a list of all current user Agreements",
      OperationId = "Agreements.List",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult<AgreementResponse>> HandleAsync(CancellationToken cancellationToken)
  {

    var response = new AgreementResponse();

    response.Agreements = (await _repository.ListAsync(new AllAgreementsWithProductAndGroupSpec()))
        .Select(agreement => new AgreementRecord(agreement.Id, _userManager.Users.FirstOrDefault(u => u.Id == agreement.UserId)?.UserName, agreement.Product.ProductGroup.GroupCode, 
        agreement.Product.ProductNumber, agreement.EffectiveDate, agreement.ExpirationDate, 
        agreement.ProductPrice, agreement.NewPrice, agreement.Active, agreement.Product.Description, agreement.Product.ProductGroup.Description))
        .ToList();

    return Ok(response);
  }
}

public class AgreementResponse
{
  public List<AgreementRecord> Agreements { get; set; } = new();
}

public record AgreementRecord(int Id, string UserName, string GroupCode, string ProductNumber, DateTime EffectiveDate, DateTime? ExpirationDate, 
  decimal ProductPrice, decimal NewPrice, bool Active, string ProductDescription, string GroupDescription);


