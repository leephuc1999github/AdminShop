using System.Collections.Generic;
using System.Threading.Tasks;
using view_model.Catalog.Categories;
using view_model.Catalog.Products;
using view_model.Common;

namespace admin_webapp.Services
{
    public interface IProductApiClient
    {
        Task<List<ProductResponse>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(int productGroup,CreateProductRequest request);
        Task<ProductResponse> GetById(int id);
        Task<ProductResponse> UpdateProduct(int id , int productGroup ,UpdateProductRequest request);
        Task<bool> UpdateItemIntoProduct(int id, int productId, UpdateItemRequest request);
        Task<Item> GetItemById(int id);
        Task<bool> AddItemIntoProduct(int id , AddItemRequest request);
        Task<ApiResult<bool>> DeleteProduct(int productId);

        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request);
        Task<List<ProductGroupResponse>> GetAllProductGroup();
        Task<bool> InsertImageUrl(int id, string url);
    }
}
