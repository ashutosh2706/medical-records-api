using MedicalRecords.Model;

namespace MedicalRecords.Service
{
    public interface IChartService
    {
        Task<Chart?> GetChartByID(string id);
        Task SaveChart(Chart chart);
        Task DeleteChart(Chart chart);
    }
}