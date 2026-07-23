import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { SectionListComponent } from './section-list';
import { SectionService } from '../../services/section.service';
import { AuthService } from '../../../../services/auth';
import { Section } from '../../models/ub.models';

describe('SectionList', () => {
    let component: SectionListComponent;
    let fixture: ComponentFixture<SectionListComponent>;
    let mockSectionService: jasmine.SpyObj<SectionService>;
    let mockAuthService: jasmine.SpyObj<AuthService>;

    const mockSections: Section[] = [
        { id: 1, name: 'S1', distance: 5, order: 1, startWayPointId: 1, endWayPointId: 2 },
        { id: 2, name: 'S2', distance: 10, order: 2, startWayPointId: 2, endWayPointId: 3 }
    ];

    beforeEach(async () => {
        mockSectionService = jasmine.createSpyObj('SectionService', ['getAll', 'delete', 'update', 'insertAfter', 'importCsv']);
        mockAuthService = jasmine.createSpyObj('AuthService', ['isAdmin']);
        mockSectionService.getAll.and.returnValue(of(mockSections));

        await TestBed.configureTestingModule({
            imports: [SectionListComponent],
            providers: [
                provideRouter([]),
                provideHttpClient(),
                { provide: SectionService, useValue: mockSectionService },
                { provide: AuthService, useValue: mockAuthService },
                { provide: MatDialog, useValue: { open: () => ({ afterClosed: () => of(true) }) } }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(SectionListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should load sections on init', () => {
        expect(component.sections.length).toBe(2);
    });

    it('should compute full distance correctly', () => {
        expect(component.computeFullDistance(1)).toBe(5);
        expect(component.computeFullDistance(2)).toBe(15);
    });


    it('should delete section when confirmed', () => {
        mockSectionService.delete.and.returnValue(of(void 0));
        component.deleteSection(1);
        expect(mockSectionService.delete).toHaveBeenCalledWith(1);
    });

    it('should not delete section when cancelled', () => {
        const dialog = TestBed.inject(MatDialog);
        spyOn(dialog, 'open').and.returnValue({ afterClosed: () => of(false) } as any);
        component.deleteSection(1);
        expect(mockSectionService.delete).not.toHaveBeenCalled();
    });

    it('should return isAdmin from authService', () => {
        mockAuthService.isAdmin.and.returnValue(true);
        expect(component.isAdmin()).toBeTrue();
    });
});