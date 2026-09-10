import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Registration } from './registration';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('Registration', () => {
  let component: Registration;
  let fixture: ComponentFixture<Registration>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideRouter([]), provideHttpClient()],
      imports: [Registration]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Registration);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
