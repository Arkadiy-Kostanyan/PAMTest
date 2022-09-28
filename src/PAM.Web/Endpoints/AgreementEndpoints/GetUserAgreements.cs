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
using PAM.Core.Services;

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class GetUserAgreements : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<AgreementResponse>
{

  private readonly IAgreementsSearchService _searchService;

  public GetUserAgreements(IAgreementsSearchService searchService)
  {
    _searchService = searchService;
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

    response.Agreements = await _searchService.GetAllAgreements();
    return Ok(response);
  }
}

public class AgreementResponse
{
  public List<AgreementRecord> Agreements { get; set; } = new();
}




