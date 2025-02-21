import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorsAppointmentsComponent } from './doctors-appointments.component';

describe('DoctorsAppointmentsComponent', () => {
  let component: DoctorsAppointmentsComponent;
  let fixture: ComponentFixture<DoctorsAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorsAppointmentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorsAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
