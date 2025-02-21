import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorUserListComponent } from './doctor-user-list.component';

describe('DoctorUserListComponent', () => {
  let component: DoctorUserListComponent;
  let fixture: ComponentFixture<DoctorUserListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorUserListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorUserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
