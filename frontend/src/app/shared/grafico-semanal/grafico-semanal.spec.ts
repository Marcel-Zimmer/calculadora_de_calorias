import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraficoSemanal } from './grafico-semanal';

describe('GraficoSemanal', () => {
  let component: GraficoSemanal;
  let fixture: ComponentFixture<GraficoSemanal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraficoSemanal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GraficoSemanal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
