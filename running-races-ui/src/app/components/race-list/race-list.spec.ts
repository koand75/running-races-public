import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { RaceListComponent } from './race-list';
import { RaceService } from '../../services/race';
import { AuthService } from '../../services/auth';
import { PagedResult } from '../../models/paged-result.model';
import { Race } from '../../models/race.model';

describe('RaceList', () => {
  let component: RaceListComponent;
  let fixture: ComponentFixture<RaceListComponent>;
  let mockRaceService: jasmine.SpyObj<RaceService>;
  let mockAuthService: jasmine.SpyObj<AuthService>;

  const mockRaces: PagedResult<Race> = {
    items: [
      { id: '1', name: 'Race 1', location: 'Budapest', distance: 10, date: '2025-04-05' },
      { id: '2', name: 'Race 2', location: 'Debrecen', distance: 21, date: '2025-04-05' }
    ],
    totalCount: 2,
    page: 1,
    pageSize: 50
  };

  beforeEach(async () => {
    mockRaceService = jasmine.createSpyObj('RaceService', ['getRaces', 'deleteRace']);
    mockAuthService = jasmine.createSpyObj('AuthService', ['isAdmin', 'isAuthenticated']);
    mockRaceService.getRaces.and.returnValue(of(mockRaces));
    mockAuthService.isAdmin.and.returnValue(false);

    await TestBed.configureTestingModule({
      imports: [RaceListComponent],
      providers: [
        provideHttpClient(),
        provideRouter([]),
        { provide: RaceService, useValue: mockRaceService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(false) }) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RaceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load races on init', () => {
    expect(component.races.length).toBe(2);
    expect(component.totalCount).toBe(2);
  });

  it('should be in public mode by default', () => {
    expect(component.isAdminMode).toBeFalse();
  });

  it('should clear search on onClear', () => {
    component.searchTerm = 'test';
    component.onClear();
    expect(component.searchTerm).toBe('');
  });

  it('should toggle sort direction on same column', () => {
    component.sortBy('name');
    expect(component.sortColumn).toBe('name');
    expect(component.sortDirection).toBe('asc');
    component.sortBy('name');
    expect(component.sortDirection).toBe('desc');
  });

  it('should return isAdmin from authService', () => {
    mockAuthService.isAdmin.and.returnValue(true);
    expect(component.isAdmin()).toBeTrue();
  });
});