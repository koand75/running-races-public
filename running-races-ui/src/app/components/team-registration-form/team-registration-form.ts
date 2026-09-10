import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RaceCategoryTeamDto } from '../../models/race.model';
import { RaceCategoryTeamService } from '../../services/race-category-team';

@Component({
  selector: 'app-team-registration-form',
  standalone: true,
  imports: [CommonModule, FormsModule, MatButtonModule, MatIconModule, MatInputModule, MatFormFieldModule],
  templateUrl: './team-registration-form.html',
  styleUrl: './team-registration-form.css'
})
export class TeamRegistrationForm implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private registrationService = inject(RaceCategoryTeamService);

  raceId: string = '';
  categoryId: number = 0;
  teamName: string = '';
  startTime: string = '';

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('raceId') ?? '';
    this.categoryId = Number(this.route.snapshot.paramMap.get('categoryId'));
  }

  onSubmit(): void {
    const dto: RaceCategoryTeamDto = {
      categoryId: this.categoryId,
      teamId: 0,
      teamName: this.teamName,
      startTime: this.startTime || null,
      raceName: '',
      categoryName: ''
    };
    this.registrationService.create(this.raceId, this.categoryId, dto).subscribe(() => {
      this.router.navigate(['/admin/races', this.raceId, 'categories', this.categoryId, 'registrations']);
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/races', this.raceId, 'categories', this.categoryId, 'registrations']);
  }
}