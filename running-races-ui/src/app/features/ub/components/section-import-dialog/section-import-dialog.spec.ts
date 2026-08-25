import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionImportDialog } from './section-import-dialog';
import { MatTableModule } from '@angular/material/table';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('SectionImportDialog', () => {
  let component: SectionImportDialog;
  let fixture: ComponentFixture<SectionImportDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionImportDialog, MatTableModule],
      providers: [
        { provide: MatDialogRef, useValue: { close: () => { } } },
        {
          provide: MAT_DIALOG_DATA, useValue: {
            section: { matchedStartWayPointIds: [], matchedEndWayPointIds: [] },
            type: 'start',
            wayPoints: []
          }
        }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SectionImportDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
