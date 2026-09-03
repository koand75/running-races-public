import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { PlannerComponent } from './planner';
import { SectionService } from '../../services/section.service';
import { RunnerService } from '../../services/runner.service';
import { RunnerSectionService } from '../../services/runner-section.service';
import { TeamService } from '../../services/team.service';
import { Section, Runner, RunnerSection, Team } from '../../models/ub.models';

describe('Planner', () => {
  let component: PlannerComponent;
  let fixture: ComponentFixture<PlannerComponent>;

  const mockSections: Section[] = [
    { id: 1, name: 'S1', distance: 5, order: 1, startWayPointId: 1, endWayPointId: 2 },
    { id: 2, name: 'S2', distance: 10, order: 2, startWayPointId: 2, endWayPointId: 3 },
    { id: 3, name: 'S3', distance: 3, order: 3, startWayPointId: 3, endWayPointId: 4 }
  ];

  const mockRunners: Runner[] = [
    { id: 1, teamId: 1, name: 'Futó A', basePace: 360 },
    { id: 2, teamId: 1, name: 'Futó B', basePace: 420 }
  ];

  const mockTeam: Team = { id: 1, name: 'Teszt Csapat', year: 2025, startTime: '2025-10-01T08:00:00' };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlannerComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } },
        { provide: SectionService, useValue: { getAll: () => of(mockSections) } },
        { provide: RunnerService, useValue: { getByTeam: () => of(mockRunners) } },
        { provide: RunnerSectionService, useValue: { getByTeam: () => of([]), saveAll: () => of(null) } },
        { provide: TeamService, useValue: { getById: () => of(mockTeam) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PlannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load sections and runners on init', () => {
    expect(component.sections.length).toBe(3);
    expect(component.runners.length).toBe(2);
  });

  it('should calculate cumulative km correctly', () => {
    expect(component.getCumulativeKm(1)).toBe(5);
    expect(component.getCumulativeKm(2)).toBe(15);
    expect(component.getCumulativeKm(3)).toBe(18);
  });

  it('should format pace correctly', () => {
    expect(component.formatPace(390)).toBe('6:30');
    expect(component.formatPace(360)).toBe('6:00');
  });

  it('should select and deselect runner', () => {
    component.selectRunner(mockRunners[0]);
    expect(component.selectedRunner).toEqual(mockRunners[0]);
    component.selectRunner(mockRunners[0]);
    expect(component.selectedRunner).toBeNull();
  });

  it('should assign runner on section click', () => {
    component.selectedRunner = mockRunners[0];
    component.onSectionClick(mockSections[0]);
    expect(component.assignments.has(1)).toBeTrue();
    expect(component.hasChanges).toBeTrue();
  });

  it('should remove assignment on second click', () => {
    component.selectedRunner = mockRunners[0];
    component.onSectionClick(mockSections[0]);
    component.onSectionClick(mockSections[0]);
    expect(component.assignments.has(1)).toBeFalse();
  });

  it('should swap blocks on onBlockClick', () => {
    component.assignments.set(1, { sectionId: 1, runnerId: 1, customPace: 360 });
    component.assignments.set(2, { sectionId: 2, runnerId: 2, customPace: 420 });
    const blocks = component.getBlocks();
    component.onBlockClick(blocks[0]);
    component.onBlockClick(blocks[1]);
    expect(component.assignments.get(1)?.runnerId).toBe(2);
    expect(component.assignments.get(2)?.runnerId).toBe(1);
  });

  it('should get runner name', () => {
    expect(component.getRunnerName(1)).toBe('Futó A');
    expect(component.getRunnerName(99)).toBe('?');
  });
});