import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { WayPoint } from './waypoint';
import { WayPoint as WayPointModel } from '../models/ub.models';

describe('WayPoint', () => {
  let service: WayPoint;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:7156/api/waypoint';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(WayPoint);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all waypoints', () => {
    const mockData: WayPointModel[] = [
      { id: 1, name: 'Tihany', lat: 46.91, lng: 17.88 }
    ];
    service.getAll().subscribe(data => expect(data).toEqual(mockData));
    httpMock.expectOne(apiUrl).flush(mockData);
  });

  it('should create a waypoint', () => {
    const newWp: WayPointModel = { id: 0, name: 'Keszthely', lat: 46.76, lng: 17.24 };
    service.create(newWp).subscribe(data => expect(data.name).toBe('Keszthely'));
    httpMock.expectOne({ method: 'POST', url: apiUrl }).flush(newWp);
  });

  it('should update a waypoint', () => {
    const wp: WayPointModel = { id: 1, name: 'Updated', lat: 46.0, lng: 17.0 };
    service.update(1, wp).subscribe(data => expect(data.name).toBe('Updated'));
    httpMock.expectOne({ method: 'PUT', url: `${apiUrl}/1` }).flush(wp);
  });

  it('should delete a waypoint', () => {
    service.delete(1).subscribe();
    httpMock.expectOne({ method: 'DELETE', url: `${apiUrl}/1` }).flush(null);
  });
});