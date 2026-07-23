import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink, Router, NavigationEnd } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';

import { AuthService } from '../../services/auth';
import { Race } from '../../models/race.model';
import { RaceSearchModel } from '../../models/race-search.model';
import { RaceService } from '../../services/race';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog';

@Component({
  selector: 'app-race-list',
  standalone: true,
  imports: [
    DatePipe,
    RouterLink,
    FormsModule,
    MatIconModule,
    MatPaginatorModule
  ],

  templateUrl: './race-list.html',
  styleUrl: './race-list.css'
})

export class RaceListComponent implements OnInit {
  private authService = inject(AuthService);
  private raceService = inject(RaceService);  // ← Angular 20: inject()!
  private dialog = inject(MatDialog);
  private router = inject(Router);

  races: Race[] = [];
  loading = true;
  error = '';
  totalCount = 0;
  page = 1;
  pageSize = 50;

  sortColumn: string = '';
  sortDirection: string = 'asc';

  searchTerm = '';
  searchField = 'all';

  isAdminMode = false;  // Admin vagy Public route?

  isActiveFilter: string = 'active';  // 'active', 'inactive', 'all'

  ngOnInit(): void {
    // Ellenőrizzük, hogy admin route-on vagyunk-e
    this.isAdminMode = this.router.url.startsWith('/admin') && this.authService.isAdmin();
    this.loadRaces(true);
  }

  loadRaces(isInitialLoad: boolean = false): void {
    if (isInitialLoad) {
      this.loading = true;
    }
    this.error = '';

    const searchModel: RaceSearchModel = {
      searchTerm: this.searchTerm || undefined,
      searchField: this.searchField || undefined,
      sortBy: this.sortColumn || undefined,
      sortDirection: this.sortDirection || undefined,
      isActive: this.getIsActiveValue(),
      page: this.page,
      pageSize: this.pageSize
    };

    this.raceService.getRaces(this.isAdminMode ? 'admin' : 'public', searchModel).subscribe({
      next: (data) => {
        this.races = data.items;
        this.totalCount = data.totalCount;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Hiba történt az adatok betöltésekor';
        this.loading = false;
        console.error(err);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;  // Material 0-tól indexel, backend 1-től
    this.pageSize = event.pageSize;
    this.loadRaces();
  }

  private getIsActiveValue(): boolean | undefined {
    if (this.isActiveFilter === 'active') {
      return true;  // Csak aktív versenyek
    } else if (this.isActiveFilter === 'inactive') {
      return false;  // Csak inaktív versenyek
    } else {
      return undefined;  // Mind (backend alapértelmezés: aktív, de itt explicit null küldjük)
    }
  }

  sortBy(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.loadRaces();
  }

  deleteRace(id: string, raceName: string): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Verseny törlése',
        message: `Biztosan törlöd a "${raceName}" versenyt?`,
        confirmText: 'Törlés'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.raceService.deleteRace(id).subscribe({
          next: () => {
            this.loadRaces();
          },
          error: (err) => {
            console.error('Törlési hiba:', err);
            alert('Hiba történt a törlés során');
          }
        });
      }
    });
  }

  onSearch(): void {
    this.loadRaces();
  }

  onClear(): void {
    this.searchTerm = '';
    this.searchField = 'all';
    this.loadRaces();
  }

  onIsActiveFilterChange(): void {
    this.loadRaces();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  restoreRace(race: Race): void {
    this.raceService.restoreRace(race.id).subscribe(() => {
      this.loadRaces();
    });
  }
}