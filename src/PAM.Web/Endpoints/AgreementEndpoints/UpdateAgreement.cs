﻿using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
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

  public UpdateAgreement(IRepository<Agreement> repository, IRepository<Product> prepository)
  {
    _repository = repository;
    _prepository = prepository;
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
    if (request.ProductId == null)
    {
      return BadRequest();
    }

    //check if Product with passed Id exists
    var product = await _prepository.GetByIdAsync(request.ProductId);
    if (product is null)
    {
      return BadRequest();
    }

    var existingAgreement = await _repository.GetBySpecAsync(new AgreementByIdWithProductAndGroupSpec(request.Id)); 

    if (existingAgreement == null)
    {
      return NotFound();
    }

    existingAgreement.SetProduct(product);
    existingAgreement.EffectiveDate = request.EffectiveDate.Value;
    existingAgreement.ExpirationDate = request.ExpirationDate.Value;
    existingAgreement.NewPrice = request.NewPrice;
    existingAgreement.Active = request.Active;
    
    await _repository.UpdateAsync(existingAgreement); 

    var response = new UpdateAgreementResponse(
        agreement: new AgreementRecord(existingAgreement.Id, existingAgreement.Product.ProductGroup.GroupCode,
        existingAgreement.Product.ProductNumber, existingAgreement.EffectiveDate, existingAgreement.ExpirationDate,
        existingAgreement.ProductPrice, existingAgreement.NewPrice, existingAgreement.Active)
    );
    return Ok(response);
  }
}

public class UpdateAgreementRequest
{
  public const string Route = "/Agreements";
  
  [Required]
  public int Id { get; set; }
  [Required]
  public int? ProductId { get; set; }
  [Required]
  public DateTime? EffectiveDate { get; set; }
  [Required]
  public DateTime? ExpirationDate { get; set; }
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