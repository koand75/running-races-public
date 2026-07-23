import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { TeamListComponent } from './team-list';
import { TeamService } from '../../services/team.service';
import { AuthService } from '../../../../services/auth';
import { Team } from '../../models/ub.models';
import { MatDialog } from '@angular/material/dialog';

describe('TeamList', () => {
    let component: TeamListComponent;
    let fixture: ComponentFixture<TeamListComponent>;
    let mockTeamService: jasmine.SpyObj<TeamService>;
    let mockAuthService: jasmine.SpyObj<AuthService>;

    const mockTeams: Team[] = [
        { id: 1, name: 'Csapat A', year: 2025 },
        { id: 2, name: 'Csapat B', year: 2025 }
    ];

    beforeEach(async () => {
        mockTeamService = jasmine.createSpyObj('TeamService', ['getAll', 'create', 'delete']);
        mockAuthService = jasmine.createSpyObj('AuthService', ['isAdmin']);
        mockTeamService.getAll.and.returnValue(of(mockTeams));

        await TestBed.configureTestingModule({
            imports: [TeamListComponent],
            providers: [
                provideRouter([]),
                { provide: TeamService, useValue: mockTeamService },
                { provide: AuthService, useValue: mockAuthService },
                { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(true) }) } }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(TeamListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should load teams on init', () => {
        expect(component.teams.length).toBe(2);
    });

    it('should not add team when name is empty', () => {
        component.newTeam = { name: '' };
        component.addTeam();
        expect(mockTeamService.create).not.toHaveBeenCalled();
    });

    it('should add team when name is provided', () => {
        mockTeamService.create.and.returnValue(of({} as Team));
        component.newTeam = { name: 'Új Csapat', year: 2025 };
        component.addTeam();
        expect(mockTeamService.create).toHaveBeenCalled();
    });

    it('should delete team when confirmed', () => {
        mockTeamService.delete.and.returnValue(of(void 0));
        component.deleteTeam(1);
        expect(mockTeamService.delete).toHaveBeenCalledWith(1);
    });

    it('should not delete team when cancelled', () => {
        const dialog = TestBed.inject(MatDialog);
        spyOn(dialog, 'open').and.returnValue({ afterClosed: () => of(false) } as any);
        component.deleteTeam(1);
        expect(mockTeamService.delete).not.toHaveBeenCalled();
    });

    it('should return isAdmin from authService', () => {
        mockAuthService.isAdmin.and.returnValue(true);
        expect(component.isAdmin()).toBeTrue();
    });
});