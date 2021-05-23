using System.Collections.Generic;
using System.Threading.Tasks;
using view_model.Common;
using view_model.System.Languages;

namespace admin_webapp.Services
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}
