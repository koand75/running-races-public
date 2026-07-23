import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { PublicLayoutComponent } from './public-layout';
import { AuthService } from '../../services/auth';

describe('PublicLayout', () => {
    let component: PublicLayoutComponent;
    let fixture: ComponentFixture<PublicLayoutComponent>;
    let mockAuthService: jasmine.SpyObj<AuthService>;
    const isAuthenticated$ = new BehaviorSubject<boolean>(false);

    beforeEach(async () => {
        mockAuthService = jasmine.createSpyObj('AuthService', ['logout', 'isAdmin', 'getUserRole'], {
            isAuthenticated$: isAuthenticated$.asObservable()
        });

        await TestBed.configureTestingModule({
            imports: [PublicLayoutComponent],
            providers: [
                provideRouter([]),
                provideHttpClient(),
                { provide: AuthService, useValue: mockAuthService }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(PublicLayoutComponent);
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
        component.onLogout();
        expect(mockAuthService.logout).toHaveBeenCalled();
    });

    it('should return isAdmin from authService', () => {
        mockAuthService.isAdmin.and.returnValue(true);
        expect(component.isAdmin()).toBeTrue();
    });
});