import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RaceCategoryService } from '../../services/race-category.service';
import { RaceService } from '../../services/race';
import { RaceType } from '../../features/relay-planner/models/relay-planner.models';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RaceCategory, MeasurementType } from '../../models/race-category.model';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-race-category-form',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatSelectModule,
    MatOptionModule, MatButtonModule, MatIconModule, DatePipe],
  templateUrl: './race-category-form.html',
  styleUrl: './race-category-form.css'
})
export class RaceCategoryForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private categoryService = inject(RaceCategoryService);
  private raceService = inject(RaceService);

  form: FormGroup;
  raceId: string = '';
  categoryId: number | null = null;
  isEditMode = false;
  raceName = '';
  submitting = false;
  error = '';

  raceStartDate = '';
  raceEndDate = '';

  MeasurementType = MeasurementType;
  RaceType = RaceType;

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      raceType: ['None'],
      measurement: [MeasurementType.None],
      distance: [null],
      duration: [null],
      startDateTime: [null]
    });
  }

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('raceId') ?? '';
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.categoryId = Number(id);
      this.loadCategory();
    }
    this.raceService.getRaceById(this.raceId).subscribe(race => {
      this.raceName = race.name;
      this.raceStartDate = race.startDate.split('T')[0];
      this.raceEndDate = race.endDate ? race.endDate.split('T')[0] : '';
    });
  }

  loadCategory(): void {
    this.categoryService.getAll(this.raceId).subscribe(cats => {
      const cat = cats.find(c => c.id === this.categoryId);
      if (cat) {
        this.form.patchValue(cat);
        this.form.get('raceType')?.setValue(cat.raceType);
        this.form.get('measurement')?.setValue(cat.measurement);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    const category: RaceCategory = { ...this.form.value, raceId: this.raceId };
    const op: Observable<RaceCategory | void> = this.isEditMode
      ? this.categoryService.update(this.raceId, this.categoryId!, category)
      : this.categoryService.create(this.raceId, category);
    op.subscribe({
      next: () => this.router.navigate(['/admin/races', this.raceId, 'edit']),
      error: () => { this.error = 'Hiba történt'; this.submitting = false; }
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/races', this.raceId, 'edit']);
  }
}