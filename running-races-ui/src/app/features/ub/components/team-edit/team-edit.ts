import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/ub.models';

@Component({
  selector: 'app-team-edit',
  standalone: true,
  imports: [FormsModule, RouterLink, MatButtonModule, MatFormFieldModule, MatInputModule, MatIconModule],
  templateUrl: './team-edit.html',
  styleUrl: './team-edit.css'
})
export class TeamEdit implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private teamService = inject(TeamService);

  team: Team | null = null;

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.teamService.getById(id).subscribe(t => this.team = t);
  }

  save(): void {
    if (!this.team) return;
    this.teamService.update(this.team).subscribe(() => {
      this.router.navigate(['/ub/teams']);
    });
  }
}