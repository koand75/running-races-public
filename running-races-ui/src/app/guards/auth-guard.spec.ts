import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router } from '@angular/router';
import { provideRouter } from '@angular/router';
import { authGuard } from './auth-guard';
import { AuthService } from '../services/auth';

describe('authGuard', () => {
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => authGuard(...guardParameters));

  beforeEach(() => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['getToken']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    });
  });

  it('should allow access when token exists', () => {
    mockAuthService.getToken.and.returnValue('valid-token');
    const result = executeGuard({} as any, {} as any);
    expect(result).toBeTrue();
  });

  it('should deny access and redirect when no token', () => {
    mockAuthService.getToken.and.returnValue(null);
    const result = executeGuard({} as any, {} as any);
    expect(result).toBeFalse();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });
});