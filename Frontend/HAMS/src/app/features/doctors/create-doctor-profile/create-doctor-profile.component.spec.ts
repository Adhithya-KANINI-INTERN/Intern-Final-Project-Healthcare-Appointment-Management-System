import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDoctorProfileComponent } from './create-doctor-profile.component';

describe('CreateDoctorProfileComponent', () => {
  let component: CreateDoctorProfileComponent;
  let fixture: ComponentFixture<CreateDoctorProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateDoctorProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDoctorProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
