using DigitalShop.Application.DTOs.Report;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Application.Services.Interface.Report;
using DigitalShop.Infrastructure.Repo.Interface;
using DigitalShop.Infrastructure.Repo.Interface.Report;

namespace DigitalShop.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReportResponseDTO> GetReportApp()
        {
            var metrics = await _repository.GenerateReportAsync();

            // Manual mapping (or via Mapper if you want to add it to the Mapper.cs class)
            return new ReportResponseDTO
            {
                MostSoldProduct = metrics.MostSoldProduct,
                DistinctBuyersPerProduct = metrics.DistinctBuyersPerProduct,
                NeverBoughtProducts = metrics.NeverBoughtProducts,
                UsersWhoBoughtSameProductMultipleTimes = metrics.UsersWhoBoughtSameProductMultipleTimes,
                ProductWithMostDistinctBuyers = metrics.ProductWithMostDistinctBuyers,
                UsersWhoBoughtAllProducts = metrics.UsersWhoBoughtAllProducts,
                ProductsBoughtByEveryUser = metrics.ProductsBoughtByEveryUser
            };
        }
    }
}
