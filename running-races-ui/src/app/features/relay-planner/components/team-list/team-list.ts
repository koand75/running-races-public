import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/relay-planner.models';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../../services/auth';
import { ConfirmationDialogComponent } from '../../../../components/confirmation-dialog/confirmation-dialog';
import { MatDialog } from '@angular/material/dialog';
import { RaceService } from '../../../../services/race';
import { Race } from '../../../../models/race.model';
import { MatSelectModule } from '@angular/material/select';
import { RaceType } from '../../models/relay-planner.models';

@Component({
    selector: 'app-team-list',
    standalone: true,
    imports: [CommonModule, RouterLink, MatTableModule, MatButtonModule, MatIconModule
        , FormsModule, MatInputModule, MatFormFieldModule, MatSelectModule
    ],
    templateUrl: './team-list.html',
    styleUrl: './team-list.css'
})
export class TeamListComponent implements OnInit {
    private authService = inject(AuthService);
    isAdmin(): boolean { return this.authService.isAdmin(); }
    private dialog = inject(MatDialog);

    private raceService = inject(RaceService);
    races: Race[] = [];
    selectedRaceId: string = '';

    newTeam: Partial<Team> = { name: '', year: new Date().getFullYear() };

    addTeam(): void {
        if (!this.newTeam.name) return;

        this.teamService.create(this.newTeam as Team).subscribe(() => {
            this.loadTeams();
            this.newTeam = { name: '', year: new Date().getFullYear() };
        });
    }

    private teamService = inject(TeamService);

    teams: Team[] = [];
    displayedColumns = ['name', 'year', 'startTime', 'actions'];

    ngOnInit(): void {
        this.loadTeams();
        this.loadRaces();
    }

    loadRaces(): void{
        this.raceService.getRaces( 'public', { page: 1, pageSize: 50}).subscribe( resulét =>{
            this.races = resulét.items.filter( x => x.raceType === RaceType.Relay)
        } )
    }

    loadTeams(): void {
        this.teamService.getAll().subscribe(teams => {
            this.teams = teams;
        });
    }

    deleteTeam(id: number): void {
        const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
            data: {
                title: 'Csapat törlése',
                message: 'Biztosan törlöd ezt a csapatot?',
                confirmText: 'Törlés'
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.teamService.delete(id).subscribe(() => {
                    this.loadTeams();
                });
            }
        });
    }
}