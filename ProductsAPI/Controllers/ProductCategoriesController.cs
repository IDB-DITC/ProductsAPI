using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	//[Authorize]
	public class ProductCategoriesController : ControllerBase
	{
		private readonly ProductsAPIContext _context;
		private readonly ImageUploadService imageUpload;

		public ProductCategoriesController(ProductsAPIContext context, ImageUploadService imageUpload)
		{
			_context = context;
			this.imageUpload = imageUpload;

		}

		// GET: ProductCategories
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategory()
		{
			return await _context.ProductCategory.Include(pc => pc.Products).ToListAsync();
		}

		// GET: ProductCategories/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
		{
			var productCategory = await _context.ProductCategory.Include(pc => pc.Products).SingleAsync(pc => pc.ProductCategoryID == id);

			if (productCategory == null)
			{
				return NotFound();
			}

			return productCategory;
		}

		// PUT: ProductCategories/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
		{
			if (id != productCategory.ProductCategoryID)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(productCategory);

					var itemsIdList = productCategory.Products.Select(i => i.ProductID).ToList();

					var delItems = await _context.Product.Where(i => i.ProductCategoryID == id).Where(i => !itemsIdList.Contains(i.ProductID)).ToListAsync();

					_context.Product.RemoveRange(delItems);


					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductCategoryExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return NoContent();
			}
			return BadRequest(ModelState);
		}

		// POST: ProductCategories
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
		{

			if (productCategory.ImageUpload?.ImageData != null)
			{
				//productCategory.ImagePath = await imageUpload.Upload(productCategory.ImageUpload);
				productCategory.ImagePath = productCategory.ImageUpload?.ImageData;

			}




			_context.ProductCategory.Add(productCategory);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryID }, productCategory);
		}

		// DELETE: ProductCategories/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProductCategory(int id)
		{
			var productCategory = await _context.ProductCategory.FindAsync(id);
			if (productCategory == null)
			{
				return NotFound();
			}

			_context.ProductCategory.Remove(productCategory);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProductCategoryExists(int id)
		{
			return _context.ProductCategory.Any(e => e.ProductCategoryID == id);
		}
	}
}
