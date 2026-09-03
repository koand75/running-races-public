import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { WayPoint as WayPointModel } from '../../models/ub.models';

@Component({
  selector: 'app-waypoint-dialog',
  standalone: true,
  imports: [FormsModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule],
  templateUrl: './waypoint-dialog.html',
  styleUrl: './waypoint-dialog.css'
})
export class WaypointDialog {
  waypoint: Partial<WayPointModel> = {};

  constructor(
    public dialogRef: MatDialogRef<WaypointDialog>,
    @Inject(MAT_DIALOG_DATA) public data: WayPointModel | null
  ) {
    if (data) this.waypoint = { ...data };
  }

  save(): void {
    this.dialogRef.close(this.waypoint);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}