import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraficoDiario } from './grafico-diario';

describe('GraficoDiario', () => {
  let component: GraficoDiario;
  let fixture: ComponentFixture<GraficoDiario>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraficoDiario]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GraficoDiario);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
