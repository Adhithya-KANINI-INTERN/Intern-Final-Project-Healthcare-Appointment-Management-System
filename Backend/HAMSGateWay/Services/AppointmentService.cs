using HAMSGateway.DTOs;
using HAMSGateWay.Models;

namespace HAMSGateWay.Services
{
    public class AppointmentService
    {
        private readonly string baseUrl = "https://localhost:7033/api/"; 


        public async Task<List<AppointmentDTO>> GetAllAppointments()
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var getAppointments = await client.GetAsync($"Appointment/all-appointments");

                if (getAppointments.IsSuccessStatusCode)
                {
                    appointments = await getAppointments.Content.ReadFromJsonAsync<List<AppointmentDTO>>();
                }
            }
            return appointments;
        }

        public async Task<int> GetTotalConfirmedAppointments()
        {
            int totalConfirmed = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl); 

                var response = await client.GetAsync("Appointment/total-appointments");

                if (response.IsSuccessStatusCode)
                {
                    totalConfirmed = await response.Content.ReadFromJsonAsync<int>();

                }
            }
            return totalConfirmed;
        }


        public async Task<List<AppointmentDTO>> GetDoctorAppointments(int doctorId)
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var getAppointments = await client.GetAsync($"Appointment/doctor-appointment/{doctorId}");

                if (getAppointments.IsSuccessStatusCode)
                {
                    appointments = await getAppointments.Content.ReadFromJsonAsync<List<AppointmentDTO>>();
                }
            }
            return appointments;
        }
        public async Task<List<AppointmentDTO>> GetAppointmentsByDoctor(int userId)
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var getAppointments = await client.GetAsync($"Appointment/by-doctor/{userId}");

                if (getAppointments.IsSuccessStatusCode)
                {
                    appointments = await getAppointments.Content.ReadFromJsonAsync<List<AppointmentDTO>>();
                }
            }
            return appointments;
        }
        

        public async Task<List<AppointmentDTO>> GetAppointmentsByPatient(int patientId)
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var getAppointments = await client.GetAsync($"Appointment/patient/{patientId}");

                if (getAppointments.IsSuccessStatusCode)
                {
                    appointments = await getAppointments.Content.ReadFromJsonAsync<List<AppointmentDTO>>();
                }
            }
            return appointments;
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsPatient()
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var getAppointments = await client.GetAsync($"Appointment/patient-appointment");

                if (getAppointments.IsSuccessStatusCode)
                {
                    appointments = await getAppointments.Content.ReadFromJsonAsync<List<AppointmentDTO>>();
                }
            }
            return appointments;
        }

        public async Task<bool> CreateAppointment(AppointmentCreateDTO createDto)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PostAsJsonAsync("Appointment/create", createDto);
                Console.WriteLine($"Final URL: {baseUrl}Appointment/create");

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }

        public async Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PutAsJsonAsync($"Appointment/update/{updateDto.AppointmentId}", updateDto);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }

        public async Task<bool> CancelAppointment(AppointmentCancelDTO cancelDto)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PutAsJsonAsync($"Appointment/Cancel/{cancelDto.AppointmentId}", cancelDto);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }

        public async Task<bool> MarkAppointmentAsCompleted(int appointmentId)
        {
            bool success = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PutAsJsonAsync($"Appointment/completed/{appointmentId}", appointmentId);

                if (result.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
            return success;
        }
    }
}
