import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { TeamDetailComponent } from './team-detail';
import { TeamService } from '../../services/team.service';
import { RunnerService } from '../../services/runner.service';
import { Team, Runner } from '../../models/ub.models';
import { MatDialog } from '@angular/material/dialog';

describe('TeamDetail', () => {
    let component: TeamDetailComponent;
    let fixture: ComponentFixture<TeamDetailComponent>;
    let mockTeamService: jasmine.SpyObj<TeamService>;
    let mockRunnerService: jasmine.SpyObj<RunnerService>;

    const mockTeam: Team = { id: 1, name: 'Teszt Csapat', year: 2025 };
    const mockRunners: Runner[] = [
        { id: 1, teamId: 1, name: 'Futó A', basePace: 360 },
        { id: 2, teamId: 1, name: 'Futó B', basePace: 420 }
    ];

    beforeEach(async () => {
        mockTeamService = jasmine.createSpyObj('TeamService', ['getById']);
        mockRunnerService = jasmine.createSpyObj('RunnerService', ['getByTeam', 'create', 'update', 'delete']);
        mockTeamService.getById.and.returnValue(of(mockTeam));
        mockRunnerService.getByTeam.and.returnValue(of(mockRunners));

        await TestBed.configureTestingModule({
            imports: [TeamDetailComponent],
            providers: [
                provideRouter([]),
                { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } },
                { provide: TeamService, useValue: mockTeamService },
                { provide: RunnerService, useValue: mockRunnerService },
                { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(true) }) } }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(TeamDetailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should load team and runners on init', () => {
        expect(component.team).toEqual(mockTeam);
        expect(component.runners.length).toBe(2);
    });

    it('should format pace correctly', () => {
        expect(component.formatPace(390)).toBe('6:30');
        expect(component.formatPace(360)).toBe('6:00');
    });

    it('should parse pace correctly', () => {
        expect(component.parsePace('6:30')).toBe(390);
        expect(component.parsePace('6:00')).toBe(360);
    });

    it('should not add runner when name is empty', () => {
        component.newRunner = { name: '' };
        component.addRunner();
        expect(mockRunnerService.create).not.toHaveBeenCalled();
    });

    it('should add runner when name is provided', () => {
        mockRunnerService.create.and.returnValue(of({} as Runner));
        component.newRunner = { name: 'Új Futó', basePace: 360 };
        component.addRunner();
        expect(mockRunnerService.create).toHaveBeenCalled();
    });

    it('should set editingRunner on editRunner', () => {
        component.editRunner(mockRunners[0]);
        expect(component.editingRunner).toEqual(mockRunners[0]);
    });

    it('should clear editingRunner on cancelEdit', () => {
        component.editingRunner = mockRunners[0];
        component.cancelEdit();
        expect(component.editingRunner).toBeNull();
    });

    it('should delete runner when confirmed', () => {
        mockRunnerService.delete.and.returnValue(of(void 0));
        spyOn(window, 'confirm').and.returnValue(true);
        component.deleteRunner(1);
        expect(mockRunnerService.delete).toHaveBeenCalled();
    });
});