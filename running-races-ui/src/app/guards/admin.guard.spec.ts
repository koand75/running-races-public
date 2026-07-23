import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router } from '@angular/router';
import { provideRouter } from '@angular/router';
import { adminGuard } from './admin.guard';
import { AuthService } from '../services/auth';

describe('adminGuard', () => {
    let mockAuthService: jasmine.SpyObj<AuthService>;
    let mockRouter: jasmine.SpyObj<Router>;

    const executeGuard: CanActivateFn = (...guardParameters) =>
        TestBed.runInInjectionContext(() => adminGuard(...guardParameters));

    beforeEach(() => {
        mockAuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
        mockRouter = jasmine.createSpyObj('Router', ['navigate']);

        TestBed.configureTestingModule({
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: mockAuthService },
                { provide: Router, useValue: mockRouter }
            ]
        });
    });

    it('should allow access when authenticated', () => {
        mockAuthService.isAuthenticated.and.returnValue(true);
        const result = executeGuard({} as any, { url: '/admin/races' } as any);
        expect(result).toBeTrue();
    });

    it('should deny access and redirect to login when not authenticated', () => {
        mockAuthService.isAuthenticated.and.returnValue(false);
        const result = executeGuard({} as any, { url: '/admin/races' } as any);
        expect(result).toBeFalse();
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/login'], { queryParams: { returnUrl: '/admin/races' } });
    });
});