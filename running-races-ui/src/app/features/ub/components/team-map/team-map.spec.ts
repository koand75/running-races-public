import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { TeamMapComponent } from './team-map';
import { SectionService } from '../../services/section.service';
import { RunnerService } from '../../services/runner.service';
import { RunnerSectionService } from '../../services/runner-section.service';
import { MapService } from '../../services/map';

describe('TeamMap', () => {
  let component: TeamMapComponent;
  let fixture: ComponentFixture<TeamMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamMapComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '1' } } } },
        { provide: SectionService, useValue: { getAll: () => of([]) } },
        { provide: RunnerService, useValue: { getByTeam: () => of([]) } },
        { provide: RunnerSectionService, useValue: { getByTeam: () => of([]) } },
        {
          provide: MapService, useValue: {
            initMap: () => ({ remove: () => { } }),
            getStartIcon: () => ({}),
            createMarker: () => ({ bindPopup: () => ({ on: () => ({ addTo: () => { } }) }) }),
            destroyMap: () => { }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TeamMapComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});