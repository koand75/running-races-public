import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { RaceService } from './race';
import { Race } from '../models/race.model';
import { PagedResult } from '../models/paged-result.model';

describe('RaceService', () => {
  let service: RaceService;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:7156/api/races';
  const mockRace: Race = { id: '1', name: 'Test Race', location: 'Budapest', distance: 42, date: '2025-04-05' };
  const mockPaged: PagedResult<Race> = { items: [mockRace], totalCount: 1, page: 1, pageSize: 50 };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(RaceService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get public races', () => {
    service.getRaces('public', { page: 1, pageSize: 50 }).subscribe(data => expect(data).toEqual(mockPaged));
    httpMock.expectOne(req => req.url === `${apiUrl}/public`).flush(mockPaged);
  });

  it('should get admin races', () => {
    service.getRaces('admin', { page: 1, pageSize: 50 }).subscribe(data => expect(data).toEqual(mockPaged));
    httpMock.expectOne(req => req.url === `${apiUrl}/admin`).flush(mockPaged);
  });

  it('should create race', () => {
    service.createRace(mockRace).subscribe(data => expect(data).toEqual(mockRace));
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockRace);
  });

  it('should get race by id', () => {
    service.getRaceById('1').subscribe(data => expect(data).toEqual(mockRace));
    httpMock.expectOne(`${apiUrl}/1`).flush(mockRace);
  });

  it('should update race', () => {
    service.updateRace('1', mockRace).subscribe(data => expect(data).toEqual(mockRace));
    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockRace);
  });

  it('should delete race', () => {
    service.deleteRace('1').subscribe();
    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('should restore race', () => {
    service.restoreRace('1').subscribe();
    const req = httpMock.expectOne(`${apiUrl}/1/restore`);
    expect(req.request.method).toBe('PATCH');
    req.flush(null);
  });

});