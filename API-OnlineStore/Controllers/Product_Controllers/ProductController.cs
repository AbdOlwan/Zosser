using API_OnlineStore.Common;
using API_OnlineStore.Helpers;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService productService)
        {
            _service = productService;
        }

        /// <summary>
        /// Get all products with culture-specific translations
        /// </summary>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] string culture = "ar")
        {
            try
            {
                var products = await _service.GetAllProductsAsync(culture);
                return Ok(new ApiResponse(200,  products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving products: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get a product by ID with culture-specific translations
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>Product details</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id, [FromQuery] string culture = "ar")
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id, culture);
                if (product == null)
                    return NotFound(new ApiResponse(404, $"Product with ID {id} not found"));

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving the product: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get a product by slug with culture-specific translations
        /// </summary>
        /// <param name="slug">Product slug</param>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>Product details</returns>
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ProductDTO>> GetProductBySlug(string slug, [FromQuery] string culture = "ar")
        {
            try
            {
                var product = await _service.GetProductBySlugAsync(slug, culture);
                if (product == null)
                    return NotFound(new ApiResponse(404, $"Product with slug '{slug}' not found"));

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving the product: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get products by category ID with culture-specific translations
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of products</returns>
        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int categoryId, [FromQuery] string culture = "ar")
        {
            try
            {
                var products = await _service.GetProductsByCategoryIdAsync(categoryId, culture);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving products: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get products by brand ID with culture-specific translations
        /// </summary>
        /// <param name="brandId">Brand ID</param>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of products</returns>
        [HttpGet("brand/{brandId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByBrand(int brandId, [FromQuery] string culture = "ar")
        {
            try
            {
                var products = await _service.GetProductsByBrandIdAsync(brandId, culture);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving products: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get products by type ID with culture-specific translations
        /// </summary>
        /// <param name="typeId">Type ID</param>
        /// <param name="culture">Culture code (default: ar)</param>
        /// <returns>List of products</returns>
        [HttpGet("type/{typeId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByType(int typeId, [FromQuery] string culture = "ar")
        {
            try
            {
                var products = await _service.GetProductsByTypeIdAsync(typeId, culture);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving products: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get a product with all its translations
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product with all translations</returns>
        [HttpGet("{id:int}/translations")]
        public async Task<ActionResult<ProductWithTranslationsDTO>> GetProductWithTranslations(int id)
        {
            try
            {
                var product = await _service.GetProductWithTranslationsAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse(404, $"Product with ID {id} not found"));

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving the product: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create a new product with translations
        /// </summary>
        /// <param name="createProductDTO">Product creation data</param>
        /// <returns>Newly created product</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                // Check if a product with the same slug already exists
                if (await _service.ProductExistsBySlugAsync(createProductDTO.Slug))
                    return BadRequest(new ApiResponse(400, $"A product with slug '{createProductDTO.Slug}' already exists"));

                // Explicitly validate that translation fields are provided
                if (string.IsNullOrEmpty(createProductDTO.ArProductName) ||
                    string.IsNullOrEmpty(createProductDTO.ArDescription) ||
                    string.IsNullOrEmpty(createProductDTO.ArMarketingSlogan))
                {
                    return BadRequest(new ApiResponse(400, "Arabic translation fields (ArProductName, ArDescription, ArMarketingSlogan) are required"));
                }

                if (string.IsNullOrEmpty(createProductDTO.EnProductName) ||
                    string.IsNullOrEmpty(createProductDTO.EnDescription) ||
                    string.IsNullOrEmpty(createProductDTO.EnMarketingSlogan))
                {
                    return BadRequest(new ApiResponse(400, "English translation fields (EnProductName, EnDescription, EnMarketingSlogan) are required"));
                }

                var createdProduct = await _service.CreateProductAsync(createProductDTO);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while creating the product: {ex.Message}"));
            }
        }


        /// <summary>
        /// Update an existing product with translations
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updateProductDTO">Product update data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO updateProductDTO)
        {
            if (id != updateProductDTO.ProductId)
                return BadRequest(new ApiResponse(400, "ID in the URL does not match the ID in the request body"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                // Check if the product exists
                if (!await _service.ProductExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Product with ID {id} not found"));

                // Check if the updated slug conflicts with another product
                var existingProduct = await _service.GetProductBySlugAsync(updateProductDTO.Slug);
                if (existingProduct != null && existingProduct.ProductId != id)
                    return BadRequest(new ApiResponse(400, $"A product with slug '{updateProductDTO.Slug}' already exists"));

                await _service.UpdateProductAsync(updateProductDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while updating the product: {ex.Message}"));
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // Check if the product exists
                if (!await _service.ProductExistsAsync(id))
                    return NotFound(new ApiResponse(404, $"Product with ID {id} not found"));

                await _service.DeleteProductAsync(id);
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = $"Product With Id {id} Deleted Successfully",
                    Data = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while deleting the product: {ex.Message}"));
            }
        }
    }
}

