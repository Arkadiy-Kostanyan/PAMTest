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

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class GetUserAgreements : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<AgreementResponse>
{
  private readonly IReadRepository<Agreement> _repository;
  private readonly ICurrentUserService _currentUserService;

  public GetUserAgreements(IReadRepository<Agreement> repository, ICurrentUserService currentUserService)
  {
    _repository = repository;
    _currentUserService = currentUserService;
  }

  [Authorize]
  [HttpGet("/api/agreements")]
  [SwaggerOperation(
      Summary = "Gets a list of all current user Agreements",
      Description = "Gets a list of all current user Agreements",
      OperationId = "Agreements.List",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult<AgreementResponse>> HandleAsync(CancellationToken cancellationToken)
  {

    var response = new AgreementResponse();
    response.Agreements = (await _repository.ListAsync(new AgreementWithProductsAndGroupsSpec(_currentUserService.UserId)))
        .Select(agreement => new AgreementRecord(agreement.Id, _currentUserService.UserName, agreement.Product.ProductGroup.GroupCode, 
        agreement.Product.ProductNumber, agreement.EffectiveDate, agreement.ExpirationDate, 
        agreement.ProductPrice, agreement.NewPrice, agreement.Active))
        .ToList();

    return Ok(response);
  }
}

public class AgreementResponse
{
  public List<AgreementRecord> Agreements { get; set; } = new();
}

public record AgreementRecord(int Id, string UserName, string GroupCode, string ProductNumber, DateTime EffectiveDate, DateTime? ExpirationDate, 
  decimal ProductPrice, decimal NewPrice, bool Active);


