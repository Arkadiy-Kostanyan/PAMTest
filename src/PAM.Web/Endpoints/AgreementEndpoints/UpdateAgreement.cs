using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.Core.Interfaces;
using PAM.Core.Services;
using PAM.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


namespace PAM.Web.Endpoints.AgreementEndpoints;

public class UpdateAgreement: EndpointBaseAsync
        .WithRequest<UpdateAgreementRequest>
        .WithActionResult<UpdateAgreementResponse>
{
  private readonly IRepository<Agreement> _repository;
  private readonly IRepository<Product> _prepository;
  private readonly ICurrentUserService _currentUserService;

  public UpdateAgreement(IRepository<Agreement> repository, IRepository<Product> prepository, ICurrentUserService currentUserService)
  {
    _repository = repository;
    _prepository = prepository;
    _currentUserService = currentUserService;
  }

  [Authorize]
  [HttpPut(UpdateAgreementRequest.Route)]
  [SwaggerOperation(
      Summary = "Updates an Agreement",
      Description = "Updates an Agreement",
      OperationId = "Agreements.Update",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult<UpdateAgreementResponse>> HandleAsync(UpdateAgreementRequest request,
      CancellationToken cancellationToken)
  {
    //check if Product with passed Id exists
    var product = await _prepository.GetBySpecAsync(new ProductByIdWithGroupSpec(request.ProductId));
    if (product is null)
    {
      return BadRequest();
    }

    var existingAgreement = await _repository.GetBySpecAsync(new AgreementByIdWithProductAndGroupSpec(request.Id)); 

    if (existingAgreement == null)
    {
      return NotFound();
    }

    existingAgreement.UserId = _currentUserService.UserId;
    existingAgreement.SetProduct(product);
    existingAgreement.EffectiveDate = request.EffectiveDate;
    existingAgreement.ExpirationDate = request.ExpirationDate;
    existingAgreement.NewPrice = request.NewPrice;
    existingAgreement.Active = request.Active;
    
    await _repository.UpdateAsync(existingAgreement); 

    var response = new UpdateAgreementResponse(
        agreement: new AgreementRecord(existingAgreement.Id, _currentUserService.UserName, existingAgreement.Product.ProductGroup.GroupCode,
        existingAgreement.Product.ProductNumber, existingAgreement.EffectiveDate, existingAgreement.ExpirationDate,
        existingAgreement.ProductPrice, existingAgreement.NewPrice, existingAgreement.Active, existingAgreement.Product.Description, existingAgreement.Product.ProductGroup.Description)
    );

    return Ok(response);
  }
}

public class UpdateAgreementRequest
{
  public const string Route = "/api/v{version:apiVersion}/agreements";
  
  [Required]
  public int Id { get; set; }
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

public class UpdateAgreementResponse
{
  public UpdateAgreementResponse(AgreementRecord agreement)
  {
    Agreement = agreement;
  }
  public AgreementRecord Agreement { get; set; }
}
