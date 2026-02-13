using Catalog.Application.Comand;
using Catalog.Application.Handlers.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Spaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
     public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IMediator mediator, ILogger<CatalogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }   

        [HttpGet]
        [Route("[action]/{id}",Name ="GetProductById")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(string id)
        {
            _logger.LogInformation("GetProductById request received. ProductId: {ProductId}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("GetProductById failed - Invalid product ID: {ProductId}", id);
                return BadRequest("Id parameter cannot be null or empty.");
            }

            try
            {
                var query = new GetProductByIdQuery(id);
                var product = await _mediator.Send(query);

                if (product == null)
                {
                    _logger.LogWarning("Product not found. ProductId: {ProductId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Product retrieved successfully. ProductId: {ProductId}, Name: {ProductName}", 
                    id, product.Name);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product. ProductId: {ProductId}", id);
                throw;
            }
        }

        [HttpGet]
        [Route("[action]/{name}", Name = "GetProductByName")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> GetProductByName(string name)
        {
            _logger.LogInformation("GetProductByName request received. ProductName: {ProductName}", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("GetProductByName failed - Invalid product name: {ProductName}", name);
                return BadRequest("Name parameter cannot be null or empty.");
            }

            try
            {
                var query = new GetProductByNameQuery(name);
                var product = await _mediator.Send(query);

                if (product == null)
                {
                    _logger.LogWarning("Product not found. ProductName: {ProductName}", name);
                    return NotFound();
                }

                _logger.LogInformation("Product retrieved successfully. ProductName: {ProductName}", 
                    name);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product by name. ProductName: {ProductName}", name);
                throw;
            }
        }
        [HttpGet]
        [Route("GetAllProduct")]
        [ProducesResponseType(typeof(Pagination<ProductResponseDto>), StatusCodes.Status200OK)]
         public async Task<ActionResult<Pagination<ProductResponseDto>>> GetAllProducts([FromQuery] CatalogSpecParam spec)
        {
            _logger.LogInformation("GetAllProducts request received. PageIndex: {PageIndex}, PageSize: {PageSize}, Sort: {Sort}, Search: {Search}, BrandId: {BrandId}, TypeId: {TypeId}", 
                spec?.PageIndex, spec?.PageSize, spec?.Sort, spec?.Search, spec?.BrandId, spec?.TypeId);

            try
            {
                var query = new GetAllProductQuery(spec);
                var products = await _mediator.Send(query);

                _logger.LogInformation("GetAllProducts completed successfully. TotalCount: {TotalCount}, PageIndex: {PageIndex}, PageSize: {PageSize}, ReturnedItems: {ReturnedItems}", 
                    products?.Count, products?.PageIndex, products?.PageSize, products?.Data?.Count ?? 0);
               
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products. PageIndex: {PageIndex}, PageSize: {PageSize}", 
                    spec?.PageIndex, spec?.PageSize);
                throw;
            }
        }
        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponseDto),(int) StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody]CreatePeoductCommand createProduct)
        {
            _logger.LogInformation("CreateProduct request received. ProductName: {ProductName}, Price: {Price}", 
                createProduct?.Name, createProduct?.Price);

            if (createProduct == null)
            {
                _logger.LogWarning("CreateProduct failed - Request body is null");
                return BadRequest("Product data cannot be null.");
            }

            try
            {
                var products = await _mediator.Send<ProductResponseDto>(createProduct);

                _logger.LogInformation("Product created successfully. ProductId: {ProductId}, Name: {ProductName}", 
                    products?.Id, products?.Name);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product. ProductName: {ProductName}", createProduct?.Name);
                throw;
            }
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct([FromBody] UpdateProductCommand UpdateProduct)
        {
            _logger.LogInformation("UpdateProduct request received. ProductId: {ProductId}, ProductName: {ProductName}", 
                UpdateProduct?.Id, UpdateProduct?.Name);

            if (UpdateProduct == null)
            {
                _logger.LogWarning("UpdateProduct failed - Request body is null");
                return BadRequest("Product data cannot be null.");
            }

            try
            {
                var result = await _mediator.Send<bool>(UpdateProduct);

                if (result)
                {
                    _logger.LogInformation("Product updated successfully. ProductId: {ProductId}", UpdateProduct.Id);
                }
                else
                {
                    _logger.LogWarning("Product update failed. ProductId: {ProductId}", UpdateProduct.Id);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product. ProductId: {ProductId}", UpdateProduct?.Id);
                throw;
            }
        }

        [HttpDelete]
        [Route("{id}/DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> DeleteProduct(string id)
        {
            _logger.LogInformation("DeleteProduct request received. ProductId: {ProductId}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("DeleteProduct failed - Invalid product ID: {ProductId}", id);
                return BadRequest("Id parameter cannot be null or empty.");
            }

            try
            {
                var Command = new DeleteProductCommand(id);
                var result = await _mediator.Send<bool>(Command);

                if (result)
                {
                    _logger.LogInformation("Product deleted successfully. ProductId: {ProductId}", id);
                }
                else
                {
                    _logger.LogWarning("Product deletion failed. ProductId: {ProductId}", id);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product. ProductId: {ProductId}", id);
                throw;
            }
        }
    }
}
