import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { TeamEdit } from './team-edit';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/ub.models';

describe('TeamEdit', () => {
  let component: TeamEdit;
  let fixture: ComponentFixture<TeamEdit>;
  let mockTeamService: jasmine.SpyObj<TeamService>;

  const mockTeam: Team = { id: 1, name: 'Teszt Csapat', year: 2025, startTime: '2025-10-01T08:00:00' };

  beforeEach(async () => {
    mockTeamService = jasmine.createSpyObj('TeamService', ['getById', 'update']);
    mockTeamService.getById.and.returnValue(of(mockTeam));

    await TestBed.configureTestingModule({
      imports: [TeamEdit],
      providers: [
        provideRouter([]),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } },
        { provide: TeamService, useValue: mockTeamService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TeamEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load team on init', () => {
    expect(component.team).toEqual(mockTeam);
  });

  it('should not save when team is null', () => {
    component.team = null;
    component.save();
    expect(mockTeamService.update).not.toHaveBeenCalled();
  });

  it('should call update and navigate on save', () => {
    mockTeamService.update.and.returnValue(of(void 0));
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    component.save();
    expect(mockTeamService.update).toHaveBeenCalledWith(mockTeam);
    expect(router.navigate).toHaveBeenCalledWith(['/ub/teams']);
  });
});