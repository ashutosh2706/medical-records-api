using Azure.Core;
using MedicalRecords.Dto;
using MedicalRecords.Model;
using MedicalRecords.Service;
using MedicalRecords.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MedicalRecords.Controllers
{
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IChartService _chartService;
        private readonly IDoctorService _doctorService;

        private readonly string _directoryName = "patient_charts";
        public PatientsController(IPatientService patientService, IChartService chartService, IDoctorService doctorService)
        {
            _patientService = patientService;
            _chartService = chartService;
            _doctorService = doctorService;

        }

        [HttpGet]
        [Route("api/patients/all")]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientService.GetPatients();
                if (patients == null)
                {
                    return NotFound("Record is empty");
                }

                List<PatientResponse> patientResponses = new List<PatientResponse>();
                foreach(var patient in patients)
                {
                    PatientResponse response = new PatientResponse()
                    {
                        MR_No = patient.MRN,
                        Patient_Name = patient.FirstName + " " + patient.LastName,
                        Address = patient.Address,
                        Visits = patient.TotalVisits,
                        Chart_ID = patient.ChartID,
                        Doctor_Name = _doctorService.GetDoctorByID(patient.DoctorID).Result.DoctorName,
                        Department_Name = _doctorService.GetDoctorByID(patient.DoctorID).Result.DeptName,
                        Active = patient.IsActive
                    };
                    patientResponses.Add(response);
                }
                return Ok(patientResponses);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/patients/{mrn}")]
        public async Task<IActionResult> GetPatientDetailsByMRN(string mrn)
        {
            try
            {
                var patient = await _patientService.GetPatientByMRN(mrn);
                if (patient == null)
                {
                    return NotFound($"Patient with MR No.: {mrn} was not found.");
                }
                // create Patient Response object by collecting all the relevant data from different tables
                PatientResponse response = new PatientResponse()
                {
                    MR_No = mrn,
                    Patient_Name = patient.FirstName + " " + patient.LastName,
                    Address = patient.Address,
                    Visits = patient.TotalVisits,
                    Chart_ID = patient.ChartID,
                    Doctor_Name = _doctorService.GetDoctorByID(patient.DoctorID).Result.DoctorName,
                    Department_Name = _doctorService.GetDoctorByID(patient.DoctorID).Result.DoctorName,
                    Active = patient.IsActive

                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/patients/add")]
        public async Task<IActionResult> AddPatient([FromForm] PatientRequest patientRequest)
        {
            try
            {
                if(patientRequest.Chart == null || patientRequest.Chart.Length == 0)
                {
                    return BadRequest("New Patient chart can't be empty.");
                }

                string originalName = ContentDispositionHeaderValue.Parse(patientRequest.Chart.ContentDisposition).FileName.Trim('"');
                string chartID = Utils.UID();
                Patient patient = new Patient()
                {
                    FirstName = patientRequest.FirstName,
                    LastName = patientRequest.LastName,
                    MRN = Utils.MRN(),
                    Address = patientRequest.Address,
                    TotalVisits = 1,
                    ChartID = chartID,
                    DoctorID = patientRequest.DoctorID,
                    IsActive = patientRequest.IsActive,
                };
                string newName = $"chart_{patient.MRN}" + Path.GetExtension(originalName);
                string filePath = Path.Combine(_directoryName, newName);

                // Copy the file to the uploads folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    patientRequest.Chart.CopyTo(stream);
                }
                await _chartService.SaveChart(new Chart() { ChartID = chartID, ChartName = newName });
                await _patientService.AddPatient(patient);

                return Ok($"New Patient: [ Name: {patient.FirstName} {patient.LastName}, MRN: {patient.MRN} ] was added to the record.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/patients/update")]
        public async Task<IActionResult> UpdatePatientDetails(PatientUpdateRequest request)
        {
            try
            {
                var patient = await _patientService.GetPatientByMRN(request.MRN);
                if (patient == null)
                {
                    return NotFound($"MR No.: {request.MRN} is invalid");
                }

                patient.FirstName = request.FirstName;
                patient.LastName = request.LastName;
                patient.Address = request.Address;
                patient.DoctorID = request.DoctorID;
                patient.IsActive = request.IsActive;
                patient.TotalVisits = 1 + patient.TotalVisits;
                if (request.Chart != null && request.Chart.Length > 0)
                {
                    // delete existing chart
                    var chart = await _chartService.GetChartByID(patient.ChartID);
                    string file = Path.Combine(_directoryName, chart.ChartName);
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }

                    await _chartService.DeleteChart(chart);

                    // add new chart to database and also upload it
                    var chartId = Utils.UID();
                    var fileName = ContentDispositionHeaderValue.Parse(request.Chart.ContentDisposition).FileName.Trim('"');
                    using (var stream = new FileStream(Path.Combine(_directoryName, fileName), FileMode.Create))
                    {
                        request.Chart.CopyTo(stream);
                    }

                    await _chartService.SaveChart(new Chart() { ChartID = chartId, ChartName = fileName });
                    patient.ChartID = chartId;
                }

                await _patientService.UpdatePatient(patient);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("api/patients/delete/{mrn}")]
        public async Task<IActionResult> DeletePatient(string mrn)
        {

            try
            {
                var patient = await _patientService.GetPatientByMRN(mrn);
                if (patient == null)
                {
                    return BadRequest($"MR No.: {mrn} is invalid.");
                }

                var chart = await _chartService.GetChartByID(patient.ChartID);
                string file = Path.Combine(_directoryName, chart.ChartName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

                await _chartService.DeleteChart(chart);
                await _patientService.DeletePatient(patient);
                return Ok($"Record No.: {mrn} [ {patient.FirstName} {patient.LastName} ] was deleted successfully.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
