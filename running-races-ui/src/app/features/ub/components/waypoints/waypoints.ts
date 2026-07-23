import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RouterLink } from '@angular/router';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { WayPoint as WayPointModel } from '../../models/ub.models';
import { MatDialog } from '@angular/material/dialog';
import { WaypointDialog } from '../waypoint-dialog/waypoint-dialog';
import { AuthService } from '../../../../services/auth';
import { ConfirmationDialogComponent } from '../../../../components/confirmation-dialog/confirmation-dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-waypoints',
  standalone: true,
  imports: [CommonModule, FormsModule,
    MatTableModule, MatButtonModule,
    MatIconModule, MatInputModule,
    MatFormFieldModule, RouterLink,
    MatPaginatorModule, MatCheckboxModule    
  ],
  templateUrl: './waypoints.html',
  styleUrl: './waypoints.css'
})
export class Waypoints implements OnInit {

  private authService = inject(AuthService);
  isAdmin(): boolean { return this.authService.isAdmin(); }

  page = 1;
  pageSize = 10;
  totalCount = 0;
  pagedWaypoints: WayPointModel[] = [];

  private waypointService = inject(WayPointService);
  waypoints: WayPointModel[] = [];
  displayedColumns = ['name', 'lat', 'lng', 'actions'];
  editingWaypoint: WayPointModel | null = null;

  searchTerm = '';
  showMissingOnly = false;

  filteredWaypoints(): WayPointModel[] {
    return this.waypoints.filter(wp => {
      const matchesName = wp.name.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesMissing = !this.showMissingOnly || (!wp.lat && !wp.lng);
      return matchesName && matchesMissing;
    });
  }

  private route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['search']) {
        this.searchTerm = params['search'];
      }
      this.load();
    });
  }

  load(): void {
    this.waypointService.getAll().subscribe(wp => {
      this.waypoints = wp;
      this.totalCount = wp.length;
      this.updatePage();
    });
  }

  updatePage(): void {
    const filtered = this.filteredWaypoints();
    this.totalCount = filtered.length;
    const start = (this.page - 1) * this.pageSize;
    this.pagedWaypoints = filtered.slice(start, start + this.pageSize);
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.updatePage();
  }

  delete(id: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Váltópont törlése',
        message: 'Biztosan törlöd ezt a váltópontot?',
        confirmText: 'Törlés'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.waypointService.delete(id).subscribe({
          next: () => this.load(),
          error: (err) => alert(err.error)
        });
      }
    });
  }

  private dialog = inject(MatDialog);

  startAdd(): void {
    const dialogRef = this.dialog.open(WaypointDialog, {
      data: null,
      width: '400px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) this.waypointService.create(result as WayPointModel).subscribe(() => this.load());
    });
  }

  edit(wp: WayPointModel): void {
    const dialogRef = this.dialog.open(WaypointDialog, {
      data: wp,
      width: '400px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) this.waypointService.update(wp.id, result).subscribe(() => this.load());
    });
  }
}