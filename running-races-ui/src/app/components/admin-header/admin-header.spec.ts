import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AdminHeaderComponent } from './admin-header';
import { AuthService } from '../../services/auth';

describe('AdminHeader', () => {
  let component: AdminHeaderComponent;
  let fixture: ComponentFixture<AdminHeaderComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  const isAuthenticated$ = new BehaviorSubject<boolean>(true);

  beforeEach(async () => {
    const isAuthenticated$ = new BehaviorSubject<boolean>(true);
    mockAuthService = jasmine.createSpyObj('AuthService', ['logout', 'getUserRole', 'isAdmin'], {
      isAuthenticated$: isAuthenticated$.asObservable()
    });
    mockAuthService.getUserRole.and.returnValue('Admin');

    await TestBed.configureTestingModule({
      imports: [AdminHeaderComponent],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit menuToggled on toggleMenu', () => {
    spyOn(component.menuToggled, 'emit');
    component.toggleMenu();
    expect(component.menuToggled.emit).toHaveBeenCalled();
  });

  it('should call logout on onLogout', () => {
    component.onLogout();
    expect(mockAuthService.logout).toHaveBeenCalled();
  });
  
});