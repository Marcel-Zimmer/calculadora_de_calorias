import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { guardaPublicaGuard } from './guarda-publica-guard';

describe('guardaPublicaGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => guardaPublicaGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
