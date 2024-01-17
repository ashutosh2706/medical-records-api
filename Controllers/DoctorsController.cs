using Azure;
using MedicalRecords.Dto;
using MedicalRecords.Model;
using MedicalRecords.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalRecords.Controllers
{
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        [Route("api/doctors/all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var docs = await _doctorService.GetAllDoctors();
                List<DoctorResponse> responses = new List<DoctorResponse>();
                foreach (var doc in docs)
                {
                    DoctorResponse response = new DoctorResponse()
                    {
                        DoctorId = doc.DoctorID,
                        DoctorName = doc.DoctorName,
                        DepartmentName = doc.DeptName
                    };
                    responses.Add(response);
                }
                
                return Ok(responses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/doctors/{id}")]
        public async Task<IActionResult> GetDoctorByID(int id)
        {
            try
            {
                var doc = await _doctorService.GetDoctorByID(id);
                if (doc == null)
                {
                    return NotFound($"Doctor with id: {id} was not found");
                }

                DoctorResponse response = new DoctorResponse()
                {
                    DoctorId = doc.DoctorID,
                    DoctorName = doc.DoctorName,
                    DepartmentName = doc.DeptName
                    
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("api/doctors/add")]
        public async Task<IActionResult> AddDoctor(DoctorRequest request)
        {
            try
            {
                Doctor doc = new Doctor() { DoctorName = request.DoctorName, DeptName = request.DepartmentName };
                await _doctorService.AddDoctor(doc);
                return Ok($"Doctor: {doc.DoctorName} added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("api/doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var doc = await _doctorService.GetDoctorByID(id);
                if(doc == null)
                {
                    return NotFound($"Doctor with id: {id} was not found");
                }
                await _doctorService.DeleteDoctor(doc);
                return Ok($"Doctor [ {doc.DoctorName} ] was removed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
