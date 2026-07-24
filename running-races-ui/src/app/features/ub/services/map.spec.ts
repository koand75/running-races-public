import { TestBed } from '@angular/core/testing';

import { MapService } from './map';
import * as L from 'leaflet';

describe('Map', () => {
  let service: MapService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MapService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should destroy map', () => {
    const mockMap = { remove: jasmine.createSpy('remove') } as any;
    service.destroyMap(mockMap);
    expect(mockMap.remove).toHaveBeenCalled();
  });
});
