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
		Task<InstrumentDTO?> GetByIdAsync(int id);
		Task<PagedResultDTO<InstrumentDTO>> GetDeletedByPagedAsync(int page, int pageSize);
		Task<InstrumentDTO?> GetDeletedByIdAsync(int id);
		Task<InstrumentDTO>CreateAsync(CreateInstrumentDTO createInstrumentDTO, int userId);
		Task<InstrumentDTO> UpdateAsync(int id, UpdateInstrumentDTO dto, int userId);
        Task ToggleIsActiveAsync(int id, bool isActive, int userId);
        Task DeleteAsync(int id, int userId);
		Task BackupDeleteAsync(int id);
	}
}
