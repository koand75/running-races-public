import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaceCalculator } from './pace-calculator';

describe('PaceCalculator', () => {
  let component: PaceCalculator;
  let fixture: ComponentFixture<PaceCalculator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaceCalculator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaceCalculator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
