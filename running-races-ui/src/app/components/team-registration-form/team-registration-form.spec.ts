import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamRegistrationForm } from './team-registration-form';

describe('TeamRegistrationForm', () => {
  let component: TeamRegistrationForm;
  let fixture: ComponentFixture<TeamRegistrationForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamRegistrationForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamRegistrationForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
