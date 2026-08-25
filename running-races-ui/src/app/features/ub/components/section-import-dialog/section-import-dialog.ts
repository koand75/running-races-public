import { Component, Inject } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { SectionImportPreviewDto } from '../../models/ub.models';
import { WayPoint as WayPointModel } from '../../models/ub.models';
import { MatTableModule } from '@angular/material/table';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';


@Component({
  selector: 'app-section-import-dialog',
  standalone: true,
  imports: [MatDialogModule,
    MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, CommonModule,
    MatTableModule, AsyncPipe, MatAutocompleteModule,
    MatOptionModule, ReactiveFormsModule],
  templateUrl: './section-import-dialog.html',
  styleUrl: './section-import-dialog.css'
})
export class SectionImportDialog {
  constructor(
    public dialogRef: MatDialogRef<SectionImportDialog>,
    @Inject(MAT_DIALOG_DATA) public data: {
      section: SectionImportPreviewDto;
      type: 'start' | 'end';
      wayPoints: WayPointModel[];
    }
  ) { }

  close(): void {
    this.dialogRef.close();
  }
  

  extraWayPoints: WayPointModel[] = [];

  addToTable(wp: WayPointModel): void {
    if (!this.extraWayPoints.find(w => w.id === wp.id)) {
      this.extraWayPoints.push(wp);
    }
    this.wayPointControl.setValue('');
  }

  wayPointControl = new FormControl<WayPointModel | string>('');
  filteredWayPoints$!: Observable<WayPointModel[]>;

  ngOnInit(): void {
    this.filteredWayPoints$ = this.wayPointControl.valueChanges.pipe(
      startWith(''),
      map(val => {
        const search = typeof val === 'string' ? val : val?.name ?? '';
        return this.data.wayPoints.filter(w => w.name.toLowerCase().includes(search.toLowerCase()));
      })
    );
  }

  displayWayPoint(wp: WayPointModel): string {
    return wp?.name ?? '';
  }

  getWayPoint(id: number): WayPointModel | undefined {
    return this.data.wayPoints.find(wp => wp.id === id);
  }

  getTableData(): any[] {
    const csvRow = {
      name: this.data.type === 'start' ? this.data.section.startWayPointName : this.data.section.endWayPointName,
      lat: this.data.type === 'start' ? this.data.section.startLat : this.data.section.endLat,
      lng: this.data.type === 'start' ? this.data.section.startLng : this.data.section.endLng,
      isCsv: true,
      id: null
    };

    const matchIds = this.data.type === 'start'
      ? this.data.section.matchedStartWayPointIds
      : this.data.section.matchedEndWayPointIds;

    const matches = matchIds
      .map(id => this.getWayPoint(id))
      .filter(wp => wp !== undefined)
      .map(wp => ({ name: wp!.name, lat: wp!.lat, lng: wp!.lng, isCsv: false, id: wp!.id }));

    const extras = this.extraWayPoints
      .filter(wp => !matchIds.includes(wp.id))
      .map(wp => ({ name: wp.name, lat: wp.lat, lng: wp.lng, isCsv: false, id: wp.id }));

    return [csvRow, ...matches, ...extras];
  }

  select(row: any): void {
    if (!row.isCsv) {
      this.dialogRef.close(row.id);
    }
  }
}