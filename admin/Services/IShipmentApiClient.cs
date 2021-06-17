using admin_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Catalog.Shipments;
using view_model.Common;

namespace admin_webapp.Services
{
    public interface IShipmentApiClient
    {
        Task<ApiResult<PagedResult<ShipmentVm>>> GetPagings(GetShipmentPagingRequest request);
        Task<ApiResult<bool>> Update(int id, int status,string NoteShipping, float moneyShipping);
    }
}
