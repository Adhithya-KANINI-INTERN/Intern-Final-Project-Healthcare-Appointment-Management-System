using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HAMSGateWay.DTOs;
using HAMSGateWay.Models;
using Microsoft.AspNetCore.Mvc;

namespace HAMSGateWay.Services
{
    public class DoctorService
    {
        private readonly string baseUrl = "https://localhost:7033/api/"; // Your Doctor Microservice URL



        public async Task<bool> CheckDoctorProfile(int userId)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.GetAsync($"Doctor/check-profile/{userId}");

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }
        public async Task<DoctorDTO> GetDoctorProfilebyId(int doctorId)
        {
            DoctorDTO doctorProfile = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync("Doctor/profile", (HttpCompletionOption)doctorId);
                if (response.IsSuccessStatusCode)
                {
                    doctorProfile = await response.Content.ReadFromJsonAsync<DoctorDTO>();
                }
            }
            return doctorProfile;
        }

        public async Task<DoctorDTO> GetDoctorProfile(int userId)
        {
            DoctorDTO doctorProfile = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync($"Doctor/profile/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    doctorProfile = await response.Content.ReadFromJsonAsync<DoctorDTO>();
                }
            }
            return doctorProfile;
        }

        public async Task<List<DoctorDTO>> GetDoctorsBySpecialization(string specialization)
        {
            List<DoctorDTO> doctors = new List<DoctorDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync($"Doctor/specialization/{specialization}");

                if (response.IsSuccessStatusCode)
                {
                    doctors = await response.Content.ReadFromJsonAsync<List<DoctorDTO>>();
                }
            }
            return doctors;
        }


        public async Task<List<DoctorDTO>> GetAllDoctorProfile()
        {
            List<DoctorDTO> doctors = new List<DoctorDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync("Doctor/all-profile");

                if (response.IsSuccessStatusCode)
                {
                    doctors = await response.Content.ReadFromJsonAsync<List<DoctorDTO>>();
                }
            }
            return doctors;
        }

        public async Task<List<DoctorDTO>> GetPendingDoctors()
        {
            List<DoctorDTO> doctor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync("Doctor/pending");
                if (response.IsSuccessStatusCode)
                {
                    doctor = await response.Content.ReadFromJsonAsync<List<DoctorDTO>>();
                }
            }
            return doctor ?? new List<DoctorDTO>();
        }

        public async Task<bool> AddDoctorProfile(AddDoctorProfileDTO addDoctorProfileDTO)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PostAsJsonAsync("Doctor/add-profile", addDoctorProfileDTO);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }
        public async Task<bool> UpdateDoctorProfile(DoctorDTO doctorDTO)
        {

            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PutAsJsonAsync("Doctor/updateprofile", doctorDTO);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }


        public async Task<List<DoctorSlotsDTO>> GetDoctorSlots(int doctorId, DateTime date)
        {
            List<DoctorSlotsDTO> availableSlots = new List<DoctorSlotsDTO>();

            string dateStr = date.ToString("yyyy-MM-dd");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                Console.WriteLine($"Final URL: {baseUrl}Doctor/{doctorId}/doctor-slots?appointmentDate={dateStr}");

                try
                {
                    var response = await client.GetAsync($"Doctor/{doctorId}/doctor-slots?appointmentDate={dateStr}");

                    if (response.IsSuccessStatusCode)
                    {
                        availableSlots = await response.Content.ReadFromJsonAsync<List<DoctorSlotsDTO>>();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                }
            }

            return availableSlots;
        }


        public async Task<List<DoctorAvailabilitySlotDTO>> GetAvailableSlotsofDoctor(int doctorId, DateTime appointmentDate)
        {
            List<DoctorAvailabilitySlotDTO> availableSlots = new List<DoctorAvailabilitySlotDTO>();

            string appointmentDateStr = appointmentDate.ToString("yyyy-MM-dd");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                Console.WriteLine($"Final URL: {baseUrl}Doctor/{doctorId}/available-slots?appointmentDate={appointmentDateStr}");

                try
                {
                    var response = await client.GetAsync($"Doctor/{doctorId}/available-slots?appointmentDate={appointmentDateStr}");

                    if (response.IsSuccessStatusCode)
                    {
                        availableSlots = await response.Content.ReadFromJsonAsync<List<DoctorAvailabilitySlotDTO>>();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                }
            }

            return availableSlots;
        }

        public async Task<bool> ManageDoctorAvailability(DoctorAvailabilityDTO availabilityDto)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PostAsJsonAsync("Doctor/availability", availabilityDto);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }

        public async Task<bool> VerifyDoctorProfile(VerifyDoctorDTO verifyDoctorDto)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PutAsJsonAsync($"Doctor/verify-doctor", verifyDoctorDto);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }
    }
}
