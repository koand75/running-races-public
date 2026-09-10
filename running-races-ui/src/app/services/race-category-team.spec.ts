import { TestBed } from '@angular/core/testing';

import { RaceCategoryTeamService } from './race-category-team';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('RaceCategoryTeam', () => {
  let service: RaceCategoryTeamService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideRouter([]), provideHttpClient()]

    });
    service = TestBed.inject(RaceCategoryTeamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
