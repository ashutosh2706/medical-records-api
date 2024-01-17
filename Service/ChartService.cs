using MedicalRecords.Model;

namespace MedicalRecords.Service
{
    public class ChartService : IChartService
    {
        private readonly MyContext _context;
        public ChartService(MyContext context)
        {
            _context = context;
        }

        public async Task<Chart?> GetChartByID(string id)
        {
            return await _context.Charts.FindAsync(id);
        }

        // this method will be called from PatientController to save the chart to db 
        public async Task SaveChart(Chart chart)
        {
            _context.Charts.Add(chart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChart(Chart chart)
        {
            _context.Charts.Remove(chart);
            await _context.SaveChangesAsync();
        }

    }
}
