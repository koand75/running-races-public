import { Component, inject } from '@angular/core';
import { SectionService } from '../../services/section.service';
import { Section, SectionImportPreviewDto } from '../../models/ub.models';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { WayPoint as WayPointService } from '../../services/waypoint';
import { WayPoint as WayPointModel } from '../../models/ub.models';
import { SectionImportDialog } from '../section-import-dialog/section-import-dialog';
import { WayPointMatchStatus } from '../../models/ub.models';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-section-import',
  imports: [MatIcon, MatIconModule,
    MatButtonModule, MatTableModule,
    RouterLink],
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
    return this.previewResult?.every(s =>
      s.startWayPointStatus === WayPointMatchStatus.Exact &&
      s.endWayPointStatus === WayPointMatchStatus.Exact
    ) ?? false;
  }

  private dialog = inject(MatDialog);
  private waypointService = inject(WayPointService);
  wayPoints: WayPointModel[] = [];

  previewResult: SectionImportPreviewDto[] | null = null;

  onFileSelected(event: Event): void {

    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      this.sectionService.previewCsv(file).subscribe(result => {

        this.previewResult = result
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

}
