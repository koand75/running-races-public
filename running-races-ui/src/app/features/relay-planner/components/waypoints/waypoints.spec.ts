import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { Waypoints } from './waypoints';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { WayPoint as WayPointModel } from '../../models/ub.models';

describe('Waypoints', () => {
  let component: Waypoints;
  let fixture: ComponentFixture<Waypoints>;
  let mockWaypointService: jasmine.SpyObj<WayPointService>;

  const mockWaypoints: WayPointModel[] = [
    { id: 1, name: 'Balatonfüred', lat: 46.95, lng: 17.89 },
    { id: 2, name: 'Tihany', lat: 46.91, lng: 17.88 }
  ];

  beforeEach(async () => {
    mockWaypointService = jasmine.createSpyObj('WayPointService', ['getAll', 'delete']);
    mockWaypointService.getAll.and.returnValue(of(mockWaypoints));
    mockWaypointService.delete.and.returnValue(of(void 0));

    await TestBed.configureTestingModule({
      imports: [Waypoints],
      providers: [
        provideHttpClient(),
        provideRouter([]),
        { provide: WayPointService, useValue: mockWaypointService },
        { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(true) }) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Waypoints);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load waypoints on init', () => {
    expect(component.waypoints).toEqual(mockWaypoints);
  });

  it('should call delete service on delete confirmed', () => {
    mockWaypointService.delete.and.returnValue(of(undefined));
    component.delete(1);
    expect(mockWaypointService.delete).toHaveBeenCalledWith(1);
  });

  it('should not call delete service if not confirmed', () => {
    const dialog = TestBed.inject(MatDialog);
    spyOn(dialog, 'open').and.returnValue({ afterClosed: () => of(false) } as any);
    component.delete(1);
    expect(mockWaypointService.delete).not.toHaveBeenCalled();
  });

  it('should page waypoints correctly', () => {
    expect(component.pagedWaypoints.length).toBe(2);
    expect(component.totalCount).toBe(2);
  });

  it('should filter waypoints by search term', () => {
    component.searchTerm = 'Tihany';
    component.updatePage();
    expect(component.pagedWaypoints.length).toBe(1);
    expect(component.pagedWaypoints[0].name).toBe('Tihany');
  });

  it('should filter waypoints with missing coordinates', () => {
    component.waypoints = [
      { id: 1, name: 'Balatonfüred', lat: 46.95, lng: 17.89 },
      { id: 2, name: 'Tihany', lat: null, lng: null }
    ];
    component.showMissingOnly = true;
    component.updatePage();
    expect(component.pagedWaypoints.length).toBe(1);
    expect(component.pagedWaypoints[0].name).toBe('Tihany');
  });
});