import { TestBed } from '@angular/core/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { authInterceptor } from './auth.interceptor';
import { AuthService } from '../services/auth';
import { HttpClient } from '@angular/common/http';

describe('authInterceptor', () => {
    let httpMock: HttpTestingController;
    let mockAuthService: jasmine.SpyObj<AuthService>;

    beforeEach(() => {
        mockAuthService = jasmine.createSpyObj('AuthService', ['getToken']);

        TestBed.configureTestingModule({
            providers: [
                provideHttpClient(withInterceptors([authInterceptor])),
                provideHttpClientTesting(),
                { provide: AuthService, useValue: mockAuthService }
            ]
        });

        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpMock.verify());

    it('should add Authorization header when token exists', () => {
        mockAuthService.getToken.and.returnValue('test-token');

        TestBed.inject(HttpClient).get('/test').subscribe();

        const req = httpMock.expectOne('/test');
        expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
        req.flush(null);
    });

    it('should not add Authorization header when no token', () => {
        mockAuthService.getToken.and.returnValue(null);

        TestBed.inject(HttpClient).get('/test').subscribe();

        const req = httpMock.expectOne('/test');
        expect(req.request.headers.has('Authorization')).toBeFalse();
        req.flush(null);
    });
});