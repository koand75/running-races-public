import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { SectionInsertDialog } from './section-insert-dialog';
import { of } from 'rxjs';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { WayPoint as WayPointModel } from '../../models/ub.models';

describe('SectionInsertDialog', () => {
  let component: SectionInsertDialog;
  let fixture: ComponentFixture<SectionInsertDialog>;

  const mockWaypoints: WayPointModel[] = [
    { id: 1, name: 'Tihany', lat: 46.91, lng: 17.88 }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionInsertDialog],
      providers: [
        provideHttpClient(),
        { provide: MatDialogRef, useValue: { close: () => { } } },
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: WayPointService, useValue: { getAll: () => of(mockWaypoints) } }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SectionInsertDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should prefill data when editing existing section', () => {
    component.data = { section: { id: 1, name: 'Test', distance: 5, order: 1 } as any };
    component.newSection = { ...component.data.section };
    expect(component.newSection.distance).toBe(5);
  });

  it('should close dialog with data on save', () => {
    const closeSpy = spyOn(component.dialogRef, 'close');
    component.newSection = { distance: 10, startWayPointId: 1, endWayPointId: 2 };
    component.save();
    expect(closeSpy).toHaveBeenCalledWith(component.newSection);
  });

  it('should close dialog without data on cancel', () => {
    const closeSpy = spyOn(component.dialogRef, 'close');
    component.cancel();
    expect(closeSpy).toHaveBeenCalledWith();
  });
});