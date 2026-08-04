using DigitalShop.Infrastructure.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Infrastructure.Repo.Interface.Report
{
    public interface IReportRepository
    {
        Task<ReportMetrics> GenerateReportAsync();
    }
}
