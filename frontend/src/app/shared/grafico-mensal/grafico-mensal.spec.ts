import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraficoMensal } from './grafico-mensal';

describe('GraficoMensal', () => {
  let component: GraficoMensal;
  let fixture: ComponentFixture<GraficoMensal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraficoMensal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GraficoMensal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
