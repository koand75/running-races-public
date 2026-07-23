import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { PublicHeaderComponent } from './public-header';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';


describe('PublicHeader', () => {
  let component: PublicHeaderComponent;
  let fixture: ComponentFixture<PublicHeaderComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  const isAuthenticated$ = new BehaviorSubject<boolean>(false);

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['logout', 'isAdmin'], {
      isAuthenticated$: isAuthenticated$.asObservable()
    });

    await TestBed.configureTestingModule({
      imports: [PublicHeaderComponent],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PublicHeaderComponent);
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

  it('should navigate to races on goToHome', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    component.goToHome();
    expect(router.navigate).toHaveBeenCalledWith(['/races']);
  });
});