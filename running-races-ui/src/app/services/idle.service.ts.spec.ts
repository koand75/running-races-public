import { TestBed } from '@angular/core/testing';

import { IdleService } from './idle.service.ts';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('IdleServiceTs', () => {
  let service: IdleService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(),
      provideRouter([])]

    });
    service = TestBed.inject(IdleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
