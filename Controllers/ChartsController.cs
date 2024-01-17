using MedicalRecords.Service;
using MedicalRecords.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MedicalRecords.Controllers
{
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartService _chartService;
        private readonly IPatientService _patientService;
        private readonly string _directoryName = "patient_charts";
        public ChartsController(IChartService chartService, IPatientService patientService)
        {
            _chartService = chartService;
            _patientService = patientService;
        }

        [HttpGet]
        [Route("api/charts/{mrn}")]
        public async Task<IActionResult> GetChartByMRN(string mrn)
        {
            var patient = await _patientService.GetPatientByMRN(mrn);
            if(patient == null)
            {
                return NotFound($"No record was found for MR No.: {mrn}");
            }
            var chart = await _chartService.GetChartByID(patient.ChartID);
            string fileName = chart.ChartName;
            string _basePath = Path.Combine(Directory.GetCurrentDirectory(), _directoryName);
            string path = Path.Combine( _basePath, fileName );
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"Chart ({patient.ChartID}) was not found on server. Please update the patient chart.");
            }

            string content_type = Utils.ContentType(path);
            ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("inline");
            contentDisposition.FileName = fileName;
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            return PhysicalFile(path, content_type);
        }
    }
}
