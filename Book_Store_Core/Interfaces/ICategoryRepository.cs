using Book_Store_Core.Dto_s.CategoryDTO_s;
using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(CreateCategoryDTO categorydto);
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<Category> UpdateCategoryAsync(int id , UpdateCategoryDTO categorydto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
