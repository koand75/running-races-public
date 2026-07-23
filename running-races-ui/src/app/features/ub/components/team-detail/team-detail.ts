import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { TeamService } from '../../services/team.service';
import { RunnerService } from '../../services/runner.service';
import { Team, Runner } from '../../models/ub.models';
import { ConfirmationDialogComponent } from '../../../../components/confirmation-dialog/confirmation-dialog';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-team-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './team-detail.html',
  styleUrl: './team-detail.css'
})
export class TeamDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private teamService = inject(TeamService);
  private runnerService = inject(RunnerService);

  private dialog = inject(MatDialog);

  team: Team | null = null;
  runners: Runner[] = [];
  displayedColumns = ['name', 'email', 'basePace', 'notes', 'actions'];
  
  newRunner: Partial<Runner> = { name: '', email: '', basePace: 360, notes: '' };
  editingRunner: Runner | null = null;

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTeam(id);
    this.loadRunners(id);
  }

  loadTeam(id: number): void {
    this.teamService.getById(id).subscribe(team => {
      this.team = team;
    });
  }

  loadRunners(id: number): void {
    this.runnerService.getByTeam(id).subscribe(runners => {
      this.runners = runners;
    });
  }

  formatPace(seconds: number): string {
    const min = Math.floor(seconds / 60);
    const sec = seconds % 60;
    return `${min}:${sec.toString().padStart(2, '0')}`;
  }

  parsePace(pace: string): number {
    const [min, sec] = pace.split(':').map(Number);
    return min * 60 + (sec || 0);
  }

  addRunner(): void {
    if (!this.team || !this.newRunner.name) return;

    const runner: Runner = {
      id: 0,
      teamId: this.team.id,
      name: this.newRunner.name,
      email: this.newRunner.email || undefined,
      basePace: this.newRunner.basePace || 360,
      notes: this.newRunner.notes || undefined
    };

    this.runnerService.create(this.team.id, runner).subscribe(() => {
      this.loadRunners(this.team!.id);
      this.newRunner = { name: '', email: '', basePace: 360, notes: '' };
    });
  }

  editRunner(runner: Runner): void {
    this.editingRunner = { ...runner };
  }

  saveRunner(): void {
    if (!this.team || !this.editingRunner) return;

    this.runnerService.update(this.team.id, this.editingRunner).subscribe(() => {
      this.loadRunners(this.team!.id);
      this.editingRunner = null;
    });
  }

  cancelEdit(): void {
    this.editingRunner = null;
  }

  deleteRunner(id: number): void {
    if (!this.team) return;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Futó törlése',
        message: 'A futó törlésével a tervező beosztásai is törlődnek!',
        confirmText: 'Törlés'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.runnerService.delete(this.team!.id, id).subscribe(() => {
          this.loadRunners(this.team!.id);
        });
      }
    });
  }
}