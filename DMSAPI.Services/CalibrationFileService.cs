using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class CalibrationFileService : ICalibrationFileService
	{
		private readonly ICalibrationFileRepository _calibrationFileRepository;
		private readonly IMapper _mapper;
		private readonly IInstrumentCalibrationRepository _instrumentCalibrationRepository;
		private readonly IHostEnvironment _env;

		public CalibrationFileService(ICalibrationFileRepository calibrationFileRepository, IMapper mapper, IInstrumentCalibrationRepository instrumentCalibrationRepository, IHostEnvironment env)
		{
			_calibrationFileRepository = calibrationFileRepository;
			_mapper = mapper;
			_instrumentCalibrationRepository = instrumentCalibrationRepository;
			_env = env;
		}
		private static readonly SemaphoreSlim _loLock = new(1, 1);

		private async Task ConvertToPdf(string inputFilePath, string outputFolder)
		{
			Directory.CreateDirectory(outputFolder);

			var profileDir = Path.Combine(outputFolder, "lo_profile");
			Directory.CreateDirectory(profileDir);

			await _loLock.WaitAsync();
			try
			{
				using var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = @"C:\Program Files\LibreOffice\program\soffice.com",
						Arguments =
							$"-env:UserInstallation=file:///{profileDir.Replace("\\", "/")} " +
							$"--headless --nologo --nofirststartwizard --norestore " +
							$"--convert-to pdf --outdir \"{outputFolder}\" \"{inputFilePath}\"",
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						UseShellExecute = false,
						CreateNoWindow = true,
						WorkingDirectory = outputFolder
					}
				};

				process.Start();

				var stdoutTask = process.StandardOutput.ReadToEndAsync();
				var stderrTask = process.StandardError.ReadToEndAsync();

				if (!process.WaitForExit(180_000))
				{
					try { process.Kill(entireProcessTree: true); } catch { }
					throw new TimeoutException("LibreOffice dönüşümü zaman aşımına uğradı.");
				}

				await Task.WhenAll(stdoutTask, stderrTask);

				if (process.ExitCode != 0)
					throw new Exception($"LibreOffice hata. ExitCode={process.ExitCode}. STDERR={stderrTask.Result}");

				var expectedPdf = Path.Combine(outputFolder,
					Path.GetFileNameWithoutExtension(inputFilePath) + ".pdf");

				if (!File.Exists(expectedPdf))
					throw new Exception("PDF oluşturulamadı: çıktı dosyası bulunamadı.");
			}
			finally
			{
				_loLock.Release();
			}
		}
		public async Task<InstrumentCalibrationFileDTO> CreateAsync(CreateCalibrationFileDTO createCalibrationFileDTO, int userId)
		{
			var calibration = await _instrumentCalibrationRepository.GetByIdAsync(createCalibrationFileDTO.CalibrationId);
			var entity = _mapper.Map<DMSAPI.Entities.Models.InstrumentCalibrationFile>(createCalibrationFileDTO);
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = userId;
			entity.IsActive = true;
			entity.IsDeleted = false;

			await _calibrationFileRepository.AddAsync(entity);
			return _mapper.Map<InstrumentCalibrationFileDTO>(entity);
		} 

		public async Task<InstrumentCalibrationFileDTO?> GetCalibrationFileByIdAsync(ulong fileId)
		{
			var result = await _calibrationFileRepository.GetCalibrationFileByIdAsync(fileId);
			if (result == null)
			{
				return null;
			}
			return _mapper.Map<InstrumentCalibrationFileDTO>(result);

		}

		public async Task<PagedResultDTO<InstrumentCalibrationFileDTO>> GetCalibrationFilesByCalibrationIdAsync(int pageNumber, int pageSize)
		{
			var result = await _calibrationFileRepository.GetCalibrationFilesByCalibrationIdAsync(pageNumber, pageSize);
			var mappedItems = _mapper.Map<List<InstrumentCalibrationFileDTO>>(result.Items);
			return new PagedResultDTO<InstrumentCalibrationFileDTO>
			{
				Items = mappedItems,
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize
			};
		}

		public async Task<InstrumentCalibrationFileDTO> UploadAsync(UploadCalibrationFileDTO uploadCalibrationFileDTO, int userId)
		{
			var calibration = await _instrumentCalibrationRepository.GetByIdAsync(uploadCalibrationFileDTO.CalibrationId);
			if (calibration == null)
			{
				throw new Exception("Calibration not found");
			}
			var file = uploadCalibrationFileDTO.File;
			if(file == null || file.Length == 0)
			{
				throw new Exception("File is empty");
			}
			var folder = Path.Combine(
				 _env.ContentRootPath,
				"files",
				"calibration_files",
				uploadCalibrationFileDTO.InstrumentName
				);
			Directory.CreateDirectory(folder);
			var cleanName = Path.GetFileName(file.FileName);
			var fullPath = Path.Combine(folder, cleanName);
			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
			await ConvertToPdf(fullPath, folder);

			var pdfFileNMame = Path.GetFileNameWithoutExtension(cleanName) + ".pdf";
			var pdfFullPath = Path.Combine(folder, pdfFileNMame);
			if(!File.Exists(pdfFullPath))
			{
				throw new Exception("PDF file not found after upload");
			}

			var instrumentFolder = Path.GetFileName(uploadCalibrationFileDTO.InstrumentName);

			var originalRelPath = $"/files/calibration_files/{instrumentFolder}/{cleanName}";
			var pdfRelPath = $"/files/calibration_files/{instrumentFolder}/{pdfFileNMame}";

		
			var entity = new InstrumentCalibrationFile
			{
				CalibrationId = uploadCalibrationFileDTO.CalibrationId,
				CompanyId = calibration.CompanyId,

				FileOriginalName = file.FileName,
				FilePath = originalRelPath,
				PdfFilePath = pdfRelPath,

				FileMime = file.ContentType,
				FileSize = file.Length,
				FileType = uploadCalibrationFileDTO.FileType,
				Description = uploadCalibrationFileDTO.Description,

				CreatedBy = userId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				UpdatedBy = userId,
				IsActive = true,
				IsDeleted = false
			};
			await _calibrationFileRepository.AddAsync(entity);
			return _mapper.Map<InstrumentCalibrationFileDTO>(entity);
		}
		public async Task<(Stream Stream, string ContentType, string DownloadFileName)> DownloadAsync(ulong fileId, bool asPdf = false)
		{
			var entity = await _calibrationFileRepository.GetCalibrationFileByIdAsync(fileId)
				?? throw new Exception("Calibration file not found");

			var relPath = asPdf ? entity.PdfFilePath : entity.FilePath;

			if (string.IsNullOrWhiteSpace(relPath))
				throw new Exception(asPdf ? "PDF path is empty" : "File path is empty");

			var relative = relPath.Trim().TrimStart('/');
			relative = relative.Replace("/", Path.DirectorySeparatorChar.ToString());

			var physicalPath = Path.Combine(_env.ContentRootPath, relative);

			if (!File.Exists(physicalPath))
				throw new Exception($"Calibration file not found on disk. PhysicalPath={physicalPath}");

			var stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);

			var contentType = asPdf ? "application/pdf"
									: (!string.IsNullOrWhiteSpace(entity.FileMime) ? entity.FileMime : "application/octet-stream");

			var downloadName = asPdf
				? (Path.GetFileName(entity.PdfFilePath) ?? (Path.GetFileNameWithoutExtension(entity.FileOriginalName) + ".pdf"))
				: entity.FileOriginalName;

			return (stream, contentType, downloadName);
		}

		public async Task<bool> DeleteAsync(ulong fileId, int userId, bool deletePhysicalFiles = false)
		{
			var entity = await _calibrationFileRepository.GetCalibrationFileByIdAsync(fileId);
			if (entity == null)
			{
				throw new Exception("Calibration file not found");
			}
			entity.IsDeleted = true;
			entity.DeletedAt = DateTime.UtcNow;
			entity.DeletedBy = userId;
			await _calibrationFileRepository.UpdateAsync(entity);
			return true;
		}
		private string ToPhysicalPath(string relativeOrAbsolutePath)
		{
			var p = relativeOrAbsolutePath.Trim();
			if (Path.IsPathRooted(p)) return p;

			p = p.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
			return Path.Combine(_env.ContentRootPath, p);
		}

		private static string GetContentType(string fileName, string? fallbackMime = null)
		{
			if (!string.IsNullOrWhiteSpace(fallbackMime)) return fallbackMime;

			var ext = Path.GetExtension(fileName).ToLowerInvariant();
			return ext switch
			{
				".pdf" => "application/pdf",
				".jpg" or ".jpeg" => "image/jpeg",
				".png" => "image/png",
				".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				_ => "application/octet-stream"
			};
		}

		
	}
}
