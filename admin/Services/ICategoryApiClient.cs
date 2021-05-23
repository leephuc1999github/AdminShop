using System.Collections.Generic;
using System.Threading.Tasks;
using view_model.Catalog.Categories;

namespace admin_webapp.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll(string languageId);
    }
}
