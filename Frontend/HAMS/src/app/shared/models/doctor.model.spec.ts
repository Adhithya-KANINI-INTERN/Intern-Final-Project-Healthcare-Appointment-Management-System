import { DoctorDTO } from './doctor.model';

describe('Doctor', () => {
  it('should create an instance', () => {
    expect(new DoctorDTO()).toBeTruthy();
  });
});
