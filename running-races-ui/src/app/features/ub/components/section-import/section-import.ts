import { Component, inject } from '@angular/core';
import { SectionService } from '../../services/section.service';
import { Section, SectionImportPreviewDto, WayPointIssueDto } from '../../models/ub.models';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { WayPoint as WayPointModel } from '../../models/ub.models';
import { SectionImportDialog } from '../section-import-dialog/section-import-dialog';
import { WayPointMatchStatus } from '../../models/ub.models';
import { ConfirmationDialogComponent } from '../../../../components/confirmation-dialog/confirmation-dialog';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';


@Component({
  selector: 'app-section-import',
  imports: [MatIcon, MatIconModule,
    MatButtonModule, MatTableModule, MatOptionModule,
    MatAutocompleteModule, MatFormFieldModule, MatInputModule],
  templateUrl: './section-import.html',
  styleUrl: './section-import.css',
})

export class SectionImport {
  private sectionService = inject(SectionService);
  sections: Section[] = [];
  selectedFile: File | null = null;

  loadSections(): void {
    this.sectionService.getAll().subscribe(sections => {
      this.sections = sections;
    });
  }

  canImport(): boolean {
    if (this.wayPointIssues && this.wayPointIssues.length > 0) return false;
    return this.previewResult?.every(s =>
      s.startWayPointStatus === WayPointMatchStatus.Exact &&
      s.endWayPointStatus === WayPointMatchStatus.Exact
    ) ?? false;
  }

  resolvedIssues = new Map<string, 'keep' | 'overwrite' | 'discard'>();
  private dialog = inject(MatDialog);
  private waypointService = inject(WayPointService);
  wayPoints: WayPointModel[] = [];

  previewResult: SectionImportPreviewDto[] | null = null;
  wayPointIssues: WayPointIssueDto[] | null = null;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      this.sectionService.previewCsv(file).subscribe(result => {
        this.previewResult = result.sections;
        this.wayPointIssues = result.wayPointIssues;
      });
    }
  }

  displayedColumns = ['order', 'name', 'distance'];

  openDetail(section: SectionImportPreviewDto, type: 'start' | 'end'): void {
    const dialogRef = this.dialog.open(SectionImportDialog, {
      data: { section, type, wayPoints: this.wayPoints },
      width: '800px'
    });

    dialogRef.afterClosed().subscribe(selectedId => {
      if (selectedId !== undefined && selectedId !== null) {
        if (type === 'start') {
          section.matchedStartWayPointIds = [selectedId];
          section.startWayPointStatus = WayPointMatchStatus.Exact;
        } else {
          section.matchedEndWayPointIds = [selectedId];
          section.endWayPointStatus = WayPointMatchStatus.Exact;
        }
      }
    });
  }

  ngOnInit(): void {
    this.waypointService.getAll().subscribe(wp => this.wayPoints = wp);
  }

  executeImport(): void {
    if (!this.previewResult) return;
    const importData = this.previewResult.map(s => ({
      order: s.order,
      distance: s.distance,
      description: s.description,
      startWayPointId: s.matchedStartWayPointIds[0],
      endWayPointId: s.matchedEndWayPointIds[0]
    }));
    this.sectionService.importSections(importData).subscribe(() => {
      alert('Importálás sikeres!');
    });
  }

  resolveIssue(issue: WayPointIssueDto, action: 'keep' | 'overwrite' | 'discard'): void {
    if (action === 'keep') {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        data: {
          title: 'Váltópont létrehozása',
          message: `Létrehozod a "${issue.name}" váltópontot?`,
          confirmText: 'Létrehozás'
        }
      });
      dialogRef.afterClosed().subscribe(confirmed => {
        if (confirmed) {
          this.waypointService.create({
            id: 0,
            name: issue.name ?? '',
            lat: issue.lat,
            lng: issue.lng
          }).subscribe(created => {
            // frissítjük a matchedIds-eket a sections listában
            this.updateSectionsWithNewWayPoint(issue, created.id);
            this.resolvedIssues.set(issue.name ?? '', action);
            this.wayPointIssues = this.wayPointIssues!.filter(i => i.name !== issue.name);
          });
        }
      });
    } else {
      this.resolvedIssues.set(issue.name ?? '', action);
      this.wayPointIssues = this.wayPointIssues!.filter(i => i.name !== issue.name);
    }
  }

  updateSectionsWithNewWayPoint(issue: WayPointIssueDto, newId: number): void {
    this.previewResult = this.previewResult!.map(s => {
      const updated = { ...s };
      if (s.startWayPointName === issue.name) {
        updated.matchedStartWayPointIds = [newId];
        updated.startWayPointStatus = WayPointMatchStatus.Exact;
      }
      if (s.endWayPointName === issue.name) {
        updated.matchedEndWayPointIds = [newId];
        updated.endWayPointStatus = WayPointMatchStatus.Exact;
      }
      return updated;
    });
  }

  overwriteWayPoint(issue: WayPointIssueDto, selectedId: number): void {
    // WayPoint frissítése az importált adatokkal
    this.waypointService.update(selectedId, {
      id: selectedId,
      name: issue.name ?? '',
      lat: issue.lat,
      lng: issue.lng
    }).subscribe(() => {
      this.updateSectionsWithNewWayPoint(issue, selectedId);
      this.resolvedIssues.set(issue.name ?? '', 'overwrite');
      this.wayPointIssues = this.wayPointIssues!.filter(i => i.name !== issue.name);
    });
  }

  getWayPoint(id: number): WayPointModel | undefined {
    return this.wayPoints.find(wp => wp.id === id);
  }
}
