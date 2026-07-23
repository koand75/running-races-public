import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RaceService } from '../../services/race';
import { Race } from '../../models/race.model';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth';


@Component({
  selector: 'app-race-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './race-form.html',
  styleUrl: './race-form.css'
})
export class RaceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private raceService = inject(RaceService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);

  raceForm: FormGroup;
  submitting = false;
  error = '';
  isEditMode = false;
  raceId: string | null = null;

  constructor() {
    this.raceForm = this.fb.group({
      name: ['', Validators.required],
      date: ['', Validators.required],
      location: ['', Validators.required],
      distance: [0, [Validators.required, Validators.min(0.1)]]
    });
  }

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('id');

    if (this.raceId) {
      this.isEditMode = true;
      this.loadRace(this.raceId);
    }

  }
 
  loadRace(id: string): void {
    this.raceService.getRaceById(id).subscribe({
      next: (race) => {
        // Date formázás yyyy-MM-dd formátumra (input type="date" miatt)
        const dateStr = new Date(race.date).toISOString().split('T')[0];

        this.raceForm.patchValue({
          name: race.name,
          date: dateStr,
          location: race.location,
          distance: race.distance
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
}