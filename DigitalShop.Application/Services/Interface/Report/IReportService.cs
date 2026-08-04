using DigitalShop.Application.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Application.Services.Interface.Report
{
    public interface IReportService
    {
        Task<ReportResponseDTO> GetReportApp();
    }
}
