using Microsoft.AspNetCore.Mvc;
using PAM.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

using Ardalis.ApiEndpoints;
using PAM.Core.AgreementAggregate;
using Microsoft.AspNetCore.Authorization;

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class DeleteAgreement : EndpointBaseAsync
        .WithRequest<DeleteAgreementRequest>
        .WithoutResult
{
  private readonly IRepository<Agreement> _repository;

  public DeleteAgreement(IRepository<Agreement> repository)
  {
    _repository = repository;
  }

  [Authorize]
  [HttpDelete(DeleteAgreementRequest.Route)]
  [SwaggerOperation(
      Summary = "Deletes an Agreement",
      Description = "Deletes an Agreement",
      OperationId = "Agreements.Delete",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult> HandleAsync([FromRoute] DeleteAgreementRequest request,
      CancellationToken cancellationToken)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.Id);
    if (aggregateToDelete == null) return NotFound();

    await _repository.DeleteAsync(aggregateToDelete);

    return NoContent();
  }
}

public class DeleteAgreementRequest
{
  public const string Route = "/agreements/{id:int}";
  public static string BuildRoute(int id) => Route.Replace("{id:int}", id.ToString());

  public int Id { get; set; }
}
