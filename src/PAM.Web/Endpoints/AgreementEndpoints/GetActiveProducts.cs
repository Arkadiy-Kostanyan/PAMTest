using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PAM.Web.Endpoints.AgreementEndpoints;

public class GetActiveProducts : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ProductListResponse>
{
  private readonly IReadRepository<Product> _repository;

  public GetActiveProducts(IReadRepository<Product> repository)
  {
    _repository = repository;
  }

  [HttpGet("/api/v{version:apiVersion}/products")]
  [SwaggerOperation(
      Summary = "Gets a list of all active Products",
      Description = "Gets a list of all active Products",
      OperationId = "Product.ListActive",
      Tags = new[] { "AgreementEndpoints" })
  ]
  public override async Task<ActionResult<ProductListResponse>> HandleAsync(CancellationToken cancellationToken)
  {

    var response = new ProductListResponse();
    response.Products = (await _repository.ListAsync(new ActiveProductsSpec()))
        .Select(product => new ProductRecord(product.Id, product.ProductNumber))
        .ToList();

    return Ok(response);
  }
}

public class ProductListResponse
{
  public List<ProductRecord> Products { get; set; } = new();
}

public record ProductRecord(int Id, string ProductNumber);

