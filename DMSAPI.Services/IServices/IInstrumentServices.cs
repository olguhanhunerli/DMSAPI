using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IInstrumentServices
    {
        Task<PagedResultDTO<InstrumentDTO>> GetPagedAsync(int page, int pageSize);

	}
}
