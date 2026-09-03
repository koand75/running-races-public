import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { MapComponent } from './map';
import { SectionService } from '../../services/section.service';
import { MapService } from '../../services/map';

describe('Map', () => {
  let component: MapComponent;
  let fixture: ComponentFixture<MapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MapComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        { provide: SectionService, useValue: { getAll: () => of([]) } },
        {
          provide: MapService, useValue: {
            initMap: () => ({ remove: () => { } }),
            getStartIcon: () => ({}),
            createMarker: () => ({ bindPopup: () => ({ on: () => ({ addTo: () => { } }) }) })
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MapComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});