import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { RunnerService } from './runner.service';
import { Runner } from '../models/ub.models';

describe('RunnerService', () => {
    let service: RunnerService;
    let httpMock: HttpTestingController;

    const baseUrl = 'https://localhost:7156/api';
    const mockRunner: Runner = { id: 1, teamId: 1, name: 'Futó A', basePace: 360 };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()]
        });
        service = TestBed.inject(RunnerService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpMock.verify());

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should get runners by team', () => {
        service.getByTeam(1).subscribe(data => expect(data).toEqual([mockRunner]));
        httpMock.expectOne(`${baseUrl}/team/1/runner`).flush([mockRunner]);
    });

    it('should get runner by id', () => {
        service.getById(1, 1).subscribe(data => expect(data).toEqual(mockRunner));
        httpMock.expectOne(`${baseUrl}/team/1/runner/1`).flush(mockRunner);
    });

    it('should create runner', () => {
        service.create(1, mockRunner).subscribe(data => expect(data).toEqual(mockRunner));
        const req = httpMock.expectOne(`${baseUrl}/team/1/runner`);
        expect(req.request.method).toBe('POST');
        req.flush(mockRunner);
    });

    it('should update runner', () => {
        service.update(1, mockRunner).subscribe();
        const req = httpMock.expectOne(`${baseUrl}/team/1/runner/1`);
        expect(req.request.method).toBe('PUT');
        req.flush(null);
    });

    it('should delete runner', () => {
        service.delete(1, 1).subscribe();
        const req = httpMock.expectOne(`${baseUrl}/team/1/runner/1`);
        expect(req.request.method).toBe('DELETE');
        req.flush(null);
    });
});