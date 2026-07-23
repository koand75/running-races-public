import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { WaypointDialog } from './waypoint-dialog';

describe('WaypointDialog', () => {
  let component: WaypointDialog;
  let fixture: ComponentFixture<WaypointDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WaypointDialog],
      providers: [
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: null }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WaypointDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should prefill data when editing', () => {
    component.waypoint = { id: 1, name: 'Tihany', lat: 46.91, lng: 17.88 };
    expect(component.waypoint.name).toBe('Tihany');
  });

  it('should close dialog with waypoint on save', () => {
    const closeSpy = jasmine.createSpy('close');
    component.dialogRef = { close: closeSpy } as any;
    component.waypoint = { name: 'Keszthely' };
    component.save();
    expect(closeSpy).toHaveBeenCalledWith({ name: 'Keszthely' });
  });

  it('should close dialog without data on cancel', () => {
    const closeSpy = jasmine.createSpy('close');
    component.dialogRef = { close: closeSpy } as any;
    component.cancel();
    expect(closeSpy).toHaveBeenCalledWith();
  });
});