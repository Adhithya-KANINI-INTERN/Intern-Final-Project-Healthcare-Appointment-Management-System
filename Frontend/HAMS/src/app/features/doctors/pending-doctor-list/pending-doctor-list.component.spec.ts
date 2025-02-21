import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingDoctorListComponent } from './pending-doctor-list.component';

describe('PendingDoctorListComponent', () => {
  let component: PendingDoctorListComponent;
  let fixture: ComponentFixture<PendingDoctorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PendingDoctorListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PendingDoctorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
