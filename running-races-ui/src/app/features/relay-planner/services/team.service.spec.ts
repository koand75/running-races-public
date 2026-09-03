import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TeamService } from './team.service';
import { Team } from '../models/relay-planner.models';
import { environment } from '../../../../environments/environment';

describe('TeamService', () => {
    let service: TeamService;
    let httpMock: HttpTestingController;

    const apiUrl = `${environment.apiUrl}/team`;
    const mockTeam: Team = { id: 1, name: 'Teszt Csapat', year: 2025 };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()]
        });
        service = TestBed.inject(TeamService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpMock.verify());

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should get all teams', () => {
        service.getAll().subscribe(data => expect(data).toEqual([mockTeam]));
        httpMock.expectOne(apiUrl).flush([mockTeam]);
    });

    it('should get team by id', () => {
        service.getById(1).subscribe(data => expect(data).toEqual(mockTeam));
        httpMock.expectOne(`${apiUrl}/1`).flush(mockTeam);
    });

    it('should create team', () => {
        service.create(mockTeam).subscribe(data => expect(data).toEqual(mockTeam));
        const req = httpMock.expectOne(apiUrl);
        expect(req.request.method).toBe('POST');
        req.flush(mockTeam);
    });

    it('should update team', () => {
        service.update(mockTeam).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('PUT');
        req.flush(null);
    });

    it('should delete team', () => {
        service.delete(1).subscribe();
        const req = httpMock.expectOne(`${apiUrl}/1`);
        expect(req.request.method).toBe('DELETE');
        req.flush(null);
    });
});