import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);

    // localStorage tisztítása minden teszt előtt
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify(); // Ellenőrzi: nincs függőben lévő HTTP request
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login successfully and store token', (done) => {
    // Arrange
    const email = 'test@runningraceandi.com';
    const password = 'admin123';
    const mockResponse = { token: 'fake-jwt-token' };

    // Act
    service.login(email, password).subscribe({
      next: (response: { token: string }) => {
        // Assert
        expect(response.token).toBe('fake-jwt-token');
        expect(service.getToken() as string).toBe('fake-jwt-token');
        expect(service.isAuthenticated()).toBeTrue();
        done();
      }
    });

    // Mock HTTP response
    const req = httpMock.expectOne('https://localhost:7156/api/auth/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ email, password });
    req.flush(mockResponse);
  });

  it('should logout and clear token', () => {
    // Arrange
    localStorage.setItem('jwt_token', 'fake-token');
    
    // Act
    service.logout();
    
    // Mock HTTP POST response
    const req = httpMock.expectOne('https://localhost:7156/api/auth/logout');
    expect(req.request.method).toBe('POST');
    req.flush({ message: 'Successfully logged out' });
    
    // Assert
    expect(service.getToken()).toBeNull();
    expect(service.isAuthenticated()).toBeFalse();
  });

  it('should return token when exists', () => {
    // Arrange
    localStorage.setItem('jwt_token', 'test-token');
    
    // Act
    const token = service.getToken();
    
    // Assert
    expect(token as string).toBe('test-token');
  });
  
  it('should return null when no token', () => {
    // Arrange
    localStorage.clear();
    
    // Act
    const token = service.getToken();
    
    // Assert
    expect(token).toBeNull();
  });
  it('should return true when token exists', () => {
    // Arrange
    localStorage.setItem('jwt_token', 'test-token');
    
    // Act & Assert
    expect(service.isAuthenticated()).toBeTrue();
  });
  
  it('should return false when no token', () => {
    // Arrange
    localStorage.clear();
    
    // Act & Assert
    expect(service.isAuthenticated()).toBeFalse();
  });

});