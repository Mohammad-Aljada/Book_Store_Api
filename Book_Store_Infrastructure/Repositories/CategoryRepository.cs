using Book_Store_Core.Dto_s;
using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Dto_s.CategoryDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Books)      
                    .ToListAsync();

              

                return categories.Adapt<IEnumerable<CategoryDTO>>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching categories.", ex);
            }
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Books)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return category?.Adapt<CategoryDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching category with Id {id}.", ex);
            }
        }
      
        public async Task<Category> CreateCategoryAsync(CreateCategoryDTO categorydto)
        {
            try
            {
                var category = categorydto.Adapt<Category>();
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        public async Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                category = categoryDTO.Adapt(category);
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating category with Id {id}.", ex);
            }
        }

    public  async Task<bool>  DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting category with Id {id}.", ex);

            }
           
        }

      
    }
}
