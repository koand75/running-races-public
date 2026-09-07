import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { RunnerSectionService } from './runner-section.service';
import { RunnerSection } from '../models/relay-planner.models';
import { environment } from '../../../../../src/environments/environment'

describe('RunnerSectionService', () => {
    let service: RunnerSectionService;
    let httpMock: HttpTestingController;
    const mockRaceId = '00000000-0000-0000-0000-000000000001';

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()]
        });
        service = TestBed.inject(RunnerSectionService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpMock.verify());

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should get assignments by team', () => {
        const mockData: RunnerSection[] = [
            { id: 1, sectionId: 1, runnerId: 1, customPace: 360 }
        ];
        service.getByTeam(mockRaceId, 1).subscribe(data => expect(data).toEqual(mockData));
        httpMock.expectOne(`${environment.apiUrl}/race/${mockRaceId}/team/1/assignments`).flush(mockData);
    });

    it('should save all assignments', () => {
        const assignments: RunnerSection[] = [
            { sectionId: 1, runnerId: 1, customPace: 360 }
        ];
        service.saveAll(mockRaceId, 1, assignments).subscribe();
        const req = httpMock.expectOne(`${environment.apiUrl}/race/${mockRaceId}/team/1/assignments`);
        expect(req.request.method).toBe('PUT');
        req.flush(null);
    });
});