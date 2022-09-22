using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.Core.Interfaces;
using PAM.SharedKernel.Interfaces;
using PAM.Web.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class CreateAgreement : EndpointBaseAsync
        .WithRequest<CreateAgreementRequest>
        .WithActionResult<CreateAgreementResponse>
{
  private readonly IRepository<Agreement> _repository;
  private readonly IRepository<Product> _prepository;
  private readonly ICurrentUserService _currentUserService;

  public CreateAgreement(IRepository<Agreement> repository, IRepository<Product> prepository, ICurrentUserService currentUserService)
  {
    _repository = repository;
    _prepository = prepository;
    _currentUserService = currentUserService;
  }

  [Authorize]
  [HttpPost((CreateAgreementRequest.Route))]
  [SwaggerOperation(
      Summary = "Creates a new Agreement",
      Description = "Creates a new Agreement",
      OperationId = "Agreement.Create",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult<CreateAgreementResponse>> HandleAsync(CreateAgreementRequest request,
      CancellationToken cancellationToken)
  {
    //check if Product with passed Id exists
    var product = await _prepository.GetByIdAsync(request.ProductId);
    if(product is null)
    {
      return BadRequest();
    }

    var newAgreement = new Agreement();
    newAgreement.UserId = _currentUserService.UserId;
    newAgreement.SetProduct(product);
    newAgreement.EffectiveDate = request.EffectiveDate;
    newAgreement.ExpirationDate = request.ExpirationDate;
    newAgreement.NewPrice = request.NewPrice;
    newAgreement.Active = request.Active;

    var createdItem = await _repository.AddAsync(newAgreement);

    var response = new CreateAgreementResponse
    (
        id: createdItem.Id,
        productNumber: product.ProductNumber
    );

    return Ok(response);
  }
}

 public class CreateAgreementRequest
{
  public const string Route = "/api/v{version:apiVersion}/agreements";

  [Required]
  public int ProductId { get; set; }
  [Required]
  public DateTime EffectiveDate { get; set; }
  [Required]
  public DateTime ExpirationDate { get; set; }
  [Required]
  public decimal NewPrice { get; set; }
  [Required]
  public bool Active { get; set; }

}

public class CreateAgreementResponse
{
  public CreateAgreementResponse(int id, string productNumber)
  {
    Id = id;
    ProductNumber = productNumber;
  }
  public int Id { get; set; }
  public string ProductNumber { get; set; }
}
