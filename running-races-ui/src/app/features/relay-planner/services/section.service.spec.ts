import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { SectionService } from './section.service';
import { Section } from '../models/relay-planner.models';
import { environment } from '../../../../../src/environments/environment';

describe('SectionService', () => {
    let service: SectionService;
    let httpMock: HttpTestingController;

    const mockSection: Section = { id: 1, name: 'S1', distance: 5, order: 1, startWayPointId: 1, endWayPointId: 2 };
    const mockRaceId = '00000000-0000-0000-0000-000000000001';
    const mockCategoryId = 0;
    const apiUrl = `${environment.apiUrl}/race/${mockRaceId}/category/${mockCategoryId}/section`;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()]
        });
        service = TestBed.inject(SectionService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpMock.verify());

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should get all sections', () => {
        service.getAll(mockRaceId, mockCategoryId).subscribe(data => expect(data).toEqual([mockSection]));
        httpMock.expectOne(apiUrl).flush([mockSection]);
    });

    it('should get section by id', () => {
        service.getById(mockRaceId, mockCategoryId, 1).subscribe(data => expect(data).toEqual(mockSection));
        httpMock.expectOne(`${apiUrl}/1`).flush(mockSection);
    });

    it('should create section', () => {
        service.create(mockRaceId, mockCategoryId, mockSection).subscribe(data => expect(data).toEqual(mockSection));
        const req = httpMock.expectOne(apiUrl);
        expect(req.request.method).toBe('POST');
        req.flush(mockSection);
    });

    it('should update section', () => {
        service.update(mockRaceId, mockCategoryId, mockSection).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('PUT');
        req.flush(null);
    });

    it('should delete section', () => {
        service.delete(mockRaceId, mockCategoryId, 1).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('DELETE');
        req.flush(null);
    });

    it('should insert after order', () => {
        service.insertAfter(mockRaceId, mockCategoryId, 1, mockSection).subscribe(data => expect(data).toEqual(mockSection));
        const req = httpMock.expectOne(`${apiUrl}/insert-after/1`);
        expect(req.request.method).toBe('POST');
        req.flush(mockSection);
    });

    it('should export sections as csv', () => {
        const mockBlob = new Blob(['test'], { type: 'text/csv' });
        service.exportCsv(mockRaceId, mockCategoryId).subscribe(data => expect(data).toBeInstanceOf(Blob));
        const req = httpMock.expectOne(`${apiUrl}/section-export?includeId=false`);
        expect(req.request.method).toBe('GET');
        req.flush(mockBlob);
    });

    it('should export sections with id', () => {
        const mockBlob = new Blob(['test'], { type: 'text/csv' });
        service.exportCsv(mockRaceId, mockCategoryId, true).subscribe(data => expect(data).toBeInstanceOf(Blob));
        const req = httpMock.expectOne(`${apiUrl}/section-export?includeId=true`);
        expect(req.request.method).toBe('GET');
        req.flush(mockBlob);
    });
});