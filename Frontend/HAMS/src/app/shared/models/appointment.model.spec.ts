import { AppointmentDTO } from './appointment.model';

describe('Appointment', () => {
  it('should create an instance', () => {
    expect(new AppointmentDTO()).toBeTruthy();
  });
});
