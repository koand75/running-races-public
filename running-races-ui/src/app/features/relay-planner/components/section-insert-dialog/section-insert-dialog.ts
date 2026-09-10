import { Component, Inject, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Section } from '../../models/relay-planner.models';
import { WayPoint as WayPointModel } from '../../models/relay-planner.models';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { AsyncPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-section-insert-dialog',
  standalone: true,
  imports: [FormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    AsyncPipe
  ],
  templateUrl: './section-insert-dialog.html',
  styleUrl: './section-insert-dialog.css'
})

export class SectionInsertDialog implements OnInit {
  wayPoints: WayPointModel[] = [];
  private waypointService = inject(WayPointService);
  newSection: Partial<Section> = {};
  startWayPointControl = new FormControl<WayPointModel | string>('');
  endWayPointControl = new FormControl<WayPointModel | string>('');
  filteredStartWayPoints$!: Observable<WayPointModel[]>;
  filteredEndWayPoints$!: Observable<WayPointModel[]>;
  private route = inject(ActivatedRoute);
  raceId: string = '';
  categoryId = 0;

  constructor(
    public dialogRef: MatDialogRef<SectionInsertDialog>,
    @Inject(MAT_DIALOG_DATA) public data: {
      afterOrder?: number;
      section?: Section;
      raceId: string;
      categoryId: number;

    }
  ) {
    if (data.section) {
      this.newSection = { ...data.section };
    }
  }

  displayWayPoint(wp: WayPointModel): string {
    return wp?.name ?? '';
  }

  save(): void {
    this.dialogRef.close(this.newSection);
  }

  cancel(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {
    
    this.raceId = this.data.raceId;
    this.categoryId = this.data.categoryId;
    console.log( this.raceId)

    this.waypointService.getAll(this.raceId, this.categoryId).subscribe(wp => {
      this.wayPoints = wp;
      this.filteredStartWayPoints$ = this.startWayPointControl.valueChanges.pipe(
        startWith(this.data.section?.startWayPoint ?? ''),
        map(val => {
          const search = typeof val === 'string' ? val : val?.name ?? '';
          return wp.filter(w => w.name.toLowerCase().includes(search.toLowerCase()));
        })
      );
      this.filteredEndWayPoints$ = this.endWayPointControl.valueChanges.pipe(
        startWith(this.data.section?.endWayPoint ?? ''),
        map(val => {
          const search = typeof val === 'string' ? val : val?.name ?? '';
          return wp.filter(w => w.name.toLowerCase().includes(search.toLowerCase()));
        })
      );

      if (this.data.section?.startWayPoint) {
        this.startWayPointControl.setValue(this.data.section.startWayPoint);
      }
      if (this.data.section?.endWayPoint) {
        this.endWayPointControl.setValue(this.data.section.endWayPoint);
      }
    });
  }
}
