import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionImport } from './section-import';

import { provideHttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { provideRouter } from '@angular/router';
import { SectionService } from '../../services/section.service';

describe('SectionImport', () => {
  let component: SectionImport;
  let fixture: ComponentFixture<SectionImport>;


  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionImport],
      providers: [
        provideHttpClient(),
        provideRouter([]),
        { provide: SectionService, useValue: { getAll: () => of([]), previewCsv: () => of([]) } },
        { provide: WayPointService, useValue: { getAll: () => of([]) } },
        { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(null) }) } }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SectionImport);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
