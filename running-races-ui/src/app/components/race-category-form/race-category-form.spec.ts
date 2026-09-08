import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RaceCategoryForm } from './race-category-form';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

describe('RaceCategoryForm', () => {
  let component: RaceCategoryForm;
  let fixture: ComponentFixture<RaceCategoryForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RaceCategoryForm],
      providers: [
        provideHttpClient(),
        provideRouter([]),]
    })
      .compileComponents();

    fixture = TestBed.createComponent(RaceCategoryForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
