import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RaceService } from '../../services/race';
import { Race } from '../../models/race.model';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { RaceCategory } from '../../models/race-category.model';
import { RaceCategoryService } from '../../services/race-category.service';
import { DatePipe } from '@angular/common';
import { RaceType } from '../../features/relay-planner/models/relay-planner.models';

@Component({
  selector: 'app-race-form',
  standalone: true,
  imports: [ReactiveFormsModule, MatOptionModule,
    MatFormFieldModule, MatSelectModule, MatIconModule, RouterLink, DatePipe],
  templateUrl: './race-form.html',
  styleUrl: './race-form.css'
})
export class RaceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private raceService = inject(RaceService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private raceCategoryService = inject(RaceCategoryService);
  categories: RaceCategory[] = [];

  raceForm: FormGroup;
  submitting = false;
  error = '';
  isEditMode = false;
  raceId: string | null = null;

  constructor() {
    this.raceForm = this.fb.group({
      name: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      location: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('id');

    if (this.raceId) {
      this.isEditMode = true;
      this.loadRace(this.raceId);
      this.loadCategories();
    }

  }

  loadRace(id: string): void {
    this.raceService.getRaceById(id).subscribe({
      next: (race) => {
        // Date formázás yyyy-MM-dd formátumra (input type="date" miatt)
        const startDateStr = new Date(race.startDate).toLocaleDateString('en-CA');
        const endDateStr = race.endDate && race.endDate !== '0001-01-01T00:00:00'
          ? new Date(race.endDate).toISOString().split('T')[0]
          : '';

        this.raceForm.patchValue({
          name: race.name,
          startDate: startDateStr,
          endDate: endDateStr,
          location: race.location,
          distance: race.distance,
          raceType: race.raceType ?? 0
        });
      },
      error: (err) => {
        this.error = 'Hiba történt az adatok betöltése során';
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.raceForm.invalid) {
      return;
    }

    this.submitting = true;
    this.error = '';

    const race: Race = {
      id: this.raceId || '00000000-0000-0000-0000-000000000000',
      ...this.raceForm.value
    };

    const operation = this.isEditMode
      ? this.raceService.updateRace(this.raceId!, race)
      : this.raceService.createRace(race);

    operation.subscribe({
      next: (response: Race) => {  // ← response: Race | void
        // Create: Race object (201 Created)
        // Update: Race object (200 OK)

        if (this.authService.isAuthenticated()) {
          this.router.navigate(['/admin/races']);
        } else {
          this.router.navigate(['/races']);
        }
      },
      error: (err: HttpErrorResponse) => {
        this.error = this.isEditMode
          ? 'Hiba történt a módosítás során'
          : 'Hiba történt a mentés során';
        this.submitting = false;
        console.error(err);
      }
    });
  }

  onCancel(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/admin/races']);
    } else {
      this.router.navigate(['/races']);
    }
  }

  loadCategories(): void {
    if (this.raceId) {
      this.raceCategoryService.getAll(this.raceId).subscribe(cats => {
        this.categories = cats;
      });
    }
  }

  addCategory(): void { /* dialog */ }
  editCategory(cat: RaceCategory): void { /* dialog */ }

  deleteCategory(id?: number): void {
    if (!id) return;
    this.raceCategoryService.delete(this.raceId!, id).subscribe(() => {
      this.loadCategories();
    });
  }

  getRaceTypeName(type: RaceType): string {
    switch (type) {
      case RaceType.Relay: return 'Váltó';
      case RaceType.Individual: return 'Egyéni';
      default: return 'Nincs megadva';
    }

  }
}