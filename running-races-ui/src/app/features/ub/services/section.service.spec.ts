import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { SectionService } from './section.service';
import { Section } from '../models/ub.models';

describe('SectionService', () => {
    let service: SectionService;
    let httpMock: HttpTestingController;

    const apiUrl = 'https://localhost:7156/api/section';
    const mockSection: Section = { id: 1, name: 'S1', distance: 5, order: 1, startWayPointId: 1, endWayPointId: 2 };

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
        service.getAll().subscribe(data => expect(data).toEqual([mockSection]));
        httpMock.expectOne(apiUrl).flush([mockSection]);
    });

    it('should get section by id', () => {
        service.getById(1).subscribe(data => expect(data).toEqual(mockSection));
        httpMock.expectOne(`${apiUrl}/1`).flush(mockSection);
    });

    it('should create section', () => {
        service.create(mockSection).subscribe(data => expect(data).toEqual(mockSection));
        const req = httpMock.expectOne(apiUrl);
        expect(req.request.method).toBe('POST');
        req.flush(mockSection);
    });

    it('should update section', () => {
        service.update(mockSection).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('PUT');
        req.flush(null);
    });

    it('should delete section', () => {
        service.delete(1).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('DELETE');
        req.flush(null);
    });

    it('should insert after order', () => {
        service.insertAfter(1, mockSection).subscribe(data => expect(data).toEqual(mockSection));
        const req = httpMock.expectOne(`${apiUrl}/insert-after/1`);
        expect(req.request.method).toBe('POST');
        req.flush(mockSection);
    });
});