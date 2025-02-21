import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorSpecializationComponent } from './doctor-specialization.component';

describe('DoctorSpecializationComponent', () => {
  let component: DoctorSpecializationComponent;
  let fixture: ComponentFixture<DoctorSpecializationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorSpecializationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorSpecializationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
