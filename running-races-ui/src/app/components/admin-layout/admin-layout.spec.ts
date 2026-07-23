import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { AdminLayoutComponent } from './admin-layout';
import { AuthService } from '../../services/auth';
import { BehaviorSubject } from 'rxjs';

describe('AdminLayout', () => {
    let component: AdminLayoutComponent;
    let fixture: ComponentFixture<AdminLayoutComponent>;
    let mockAuthService: jasmine.SpyObj<AuthService>;

    beforeEach(async () => {
        const isAuthenticated$ = new BehaviorSubject<boolean>(true);
        mockAuthService = jasmine.createSpyObj('AuthService', ['logout', 'isAdmin', 'isAuthenticated', 'getUserRole'], {
            isAuthenticated$: isAuthenticated$.asObservable()
        });
        mockAuthService.getUserRole.and.returnValue('Admin');
        
        await TestBed.configureTestingModule({
            imports: [AdminLayoutComponent],
            providers: [
                provideRouter([]),
                provideHttpClient(),
                { provide: AuthService, useValue: mockAuthService }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(AdminLayoutComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should toggle menu', () => {
        expect(component.menuOpen).toBeFalse();
        component.toggleMenu();
        expect(component.menuOpen).toBeTrue();
    });

    it('should close menu', () => {
        component.menuOpen = true;
        component.closeMenu();
        expect(component.menuOpen).toBeFalse();
    });

    it('should call logout on authService', () => {
        component.logout();
        expect(mockAuthService.logout).toHaveBeenCalled();
    });
});