import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RaceCategoryTeamService, RaceCategoryTeamDto } from '../../services/race-category-team';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, RouterLink, MatButtonModule, MatIconModule, MatTableModule],
  templateUrl: './registration.html',
  styleUrl: './registration.css'
})
export class Registration implements OnInit {
  private route = inject(ActivatedRoute);
  private registrationService = inject(RaceCategoryTeamService);

  raceId: string = '';
  categoryId: number = 0;
  registrations: RaceCategoryTeamDto[] = [];
  displayedColumns = ['teamName', 'startTime', 'actions'];

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('raceId') ?? '';
    this.categoryId = Number(this.route.snapshot.paramMap.get('categoryId'));
    this.loadRegistrations();
  }

  loadRegistrations(): void {
    this.registrationService.getByCategory(this.raceId, this.categoryId).subscribe(regs => {
      this.registrations = regs;
    });
  }
  delete(id?: number): void {
    if (!id) return;
    this.registrationService.delete(this.raceId, this.categoryId, id).subscribe(() => {
      this.loadRegistrations();
    });
  }
}