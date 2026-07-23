import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RaceFormComponent } from './race-form';
import { RaceService } from '../../services/race';
import { AuthService } from '../../services/auth';
import { Race } from '../../models/race.model';
import { Router } from '@angular/router';

describe('RaceForm', () => {
  let component: RaceFormComponent;
  let fixture: ComponentFixture<RaceFormComponent>;
  let mockRaceService: jasmine.SpyObj<RaceService>;
  let mockAuthService: jasmine.SpyObj<AuthService>;

  const mockRace: Race = {
    id: '123', name: 'Test Race', location: 'Budapest',
    distance: 42.2, date: '2025-04-05'
  };

  beforeEach(async () => {
    mockRaceService = jasmine.createSpyObj('RaceService', ['getRaceById', 'createRace', 'updateRace']);
    mockAuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated']);

    await TestBed.configureTestingModule({
      imports: [RaceFormComponent],
      providers: [
        provideHttpClient(),
        provideRouter([]),
        { provide: RaceService, useValue: mockRaceService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => null } } } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RaceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize in create mode when no id', () => {
    expect(component.isEditMode).toBeFalse();
    expect(component.raceId).toBeNull();
  });

  it('should have invalid form when empty', () => {
    expect(component.raceForm.invalid).toBeTrue();
  });

  it('should have valid form when filled', () => {
    component.raceForm.patchValue({ name: 'Race', date: '2025-04-05', location: 'Budapest', distance: 10 });
    expect(component.raceForm.valid).toBeTrue();
  });

  it('should not submit when form is invalid', () => {
    component.onSubmit();
    expect(mockRaceService.createRace).not.toHaveBeenCalled();
  });

  it('should call createRace on submit in create mode', () => {
    mockRaceService.createRace.and.returnValue(of(mockRace));
    mockAuthService.isAuthenticated.and.returnValue(true);
    component.raceForm.patchValue({ name: 'Race', date: '2025-04-05', location: 'Budapest', distance: 10 });
    component.onSubmit();
    expect(mockRaceService.createRace).toHaveBeenCalled();
  });

  it('should navigate to admin races after successful create', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    mockRaceService.createRace.and.returnValue(of(mockRace));
    mockAuthService.isAuthenticated.and.returnValue(true);
    component.raceForm.patchValue({ name: 'Race', date: '2025-04-05', location: 'Budapest', distance: 10 });
    component.onSubmit();
    expect(router.navigate).toHaveBeenCalledWith(['/admin/races']);
  });
});