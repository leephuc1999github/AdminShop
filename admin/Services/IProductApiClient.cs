using System.Threading.Tasks;
using view_model.Catalog.Categories;
using view_model.Catalog.Products;
using view_model.Common;

namespace admin_webapp.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request);
        Task<ApiResult<int>> CreateProduct(ProductCreateRequest request);
        Task<ApiResult<ProductVm>> GetById(int productId , string languageId);
        Task<ApiResult<bool>> UpdateProduct(ProductUpdateRequest request);
        Task<ApiResult<bool>> DeleteProduct(int productId);
        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request);
    }
}
