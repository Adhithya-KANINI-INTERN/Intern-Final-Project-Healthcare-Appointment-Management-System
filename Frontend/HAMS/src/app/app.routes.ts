import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RemindersComponent } from './features/notifications/reminders/reminders.component';
import { LoginComponent } from './login/login.component';
import { NotificationListComponent } from './features/notifications/notification-list/notification-list.component';
import { CreateAppointmentComponent } from './features/appointments/create-appointment/create-appointment.component';
import { RegisterComponent } from './register/register.component';
import { ChangePasswordComponent } from './features/users/change-password/change-password.component';
import { ApproveAppointmentComponent } from './features/appointments/approve-appointment/approve-appointment.component';
import { UpdateAppointmentComponent } from './features/appointments/update-appointment/update-appointment.component';
import { CancelAppointmentComponent } from './features/appointments/cancel-appointment/cancel-appointment.component';
import { DoctorsAppointmentsComponent } from './features/appointments/doctors-appointments/doctors-appointments.component';
import { PatientappointmentsComponent } from './features/pages/patientappointments/patientappointments.component';
import { UserProfileComponent } from './features/users/user-profile/user-profile.component';
import { DoctorComponent } from './features/pages/doctor/doctor.component';
import { DoctorSpecializationComponent } from './features/doctors/doctor-specialization/doctor-specialization.component';
import { DoctorProfileComponent } from './features/doctors/doctor-profile/doctor-profile.component';
import { DoctorAvailabilityComponent } from './features/doctors/doctor-availability/doctor-availability.component';
import { AllAppointmentsComponent } from './features/appointments/all-appointments/all-appointments.component';
import { AdminUsersComponent } from './features/users/admin-users/admin-users.component';
import { AdminUsersUpdateComponent } from './features/users/admin-users-update/admin-users-update.component';

export const routes: Routes = [
    
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
  { path: 'dashboard', component: DashboardComponent },
  { path: 'notifications', component: NotificationListComponent },
  { path: 'reminders', component: RemindersComponent },
  { path: 'create-appointment', component: CreateAppointmentComponent },
  { path: 'approve-appointment', component: ApproveAppointmentComponent},
  { path: 'update-appointment', component: UpdateAppointmentComponent },
  { path: 'cancel-appointment', component: CancelAppointmentComponent },
  { path: 'doctors-appointments', component: DoctorsAppointmentsComponent },
  { path: 'appointment', component: PatientappointmentsComponent },
  { path: 'all-appointment', component: AllAppointmentsComponent },
  { path: 'doctor', component: DoctorComponent },
  { path: 'doctorsbyspecialization', component: DoctorSpecializationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'userprofile', component: UserProfileComponent },
  { path: 'doctor-availability', component: DoctorAvailabilityComponent },
  { path: 'doctor-profile', component: DoctorProfileComponent },
  { path: 'users', component: AdminUsersComponent },
  { path: 'update-user', component: AdminUsersUpdateComponent },
  { path: '**', redirectTo: '/dashboard' },
];
