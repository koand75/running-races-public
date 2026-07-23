import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SectionService } from '../../services/section.service';
import { Section } from '../../models/ub.models';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog } from '@angular/material/dialog';
import { SectionInsertDialog } from '../section-insert-dialog/section-insert-dialog';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../services/auth';
import { ConfirmationDialogComponent } from '../../../../components/confirmation-dialog/confirmation-dialog';

@Component({
  selector: 'app-section-list',
  standalone: true,
  imports: [CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    RouterLink
  ],
  templateUrl: './section-list.html',
  styleUrl: './section-list.css'
})
export class SectionListComponent implements OnInit {
  private authService = inject(AuthService);
  isAdmin(): boolean { return this.authService.isAdmin(); }
  private dialog = inject(MatDialog);
  private sectionService = inject(SectionService);

  sections: Section[] = [];
  displayedColumns = ['order', 'name', 'fullDistance', 'distance', 'actions'];
  editingSection: Section | null = null;
  insertingAfterOrder: number | null = null;
  newSection: Partial<Section> = {};

  ngOnInit(): void {
    this.loadSections();
  }

  loadSections(): void {
    this.sectionService.getAll().subscribe(sections => {
      this.sections = sections;
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      this.sectionService.importCsv(file).subscribe(result => {
        alert(`${result.imported} szakasz importálva`);
        this.loadSections();
      });
    }
  }

  editSection(section: Section): void {
    const dialogRef = this.dialog.open(SectionInsertDialog, {
      data: { section },
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sectionService.update({ ...section, ...result }).subscribe(() => {
          this.loadSections();
        });
      }
    });
  }

  deleteSection(id: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Szakasz törlése',
        message: 'Biztosan törlöd ezt a szakaszt?',
        confirmText: 'Törlés'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sectionService.delete(id).subscribe(() => {
          this.loadSections();
        });
      }
    });
  }

  insertAfter(order: number): void {
    const dialogRef = this.dialog.open(SectionInsertDialog, {
      data: { afterOrder: order },
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sectionService.insertAfter(order, result as Section).subscribe(() => {
          this.loadSections();
        });
      }
    });
  }

  public computeFullDistance(sectionOrderNum: number): number {
    return this.sections
      .filter(s => Number(s.order) <= sectionOrderNum)
      .reduce((sum, s) => sum + Number(s.distance), 0);
  }
}