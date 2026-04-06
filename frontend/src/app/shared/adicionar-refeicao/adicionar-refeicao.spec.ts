import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdicionarRefeicao } from './adicionar-refeicao';

describe('AdicionarRefeicao', () => {
  let component: AdicionarRefeicao;
  let fixture: ComponentFixture<AdicionarRefeicao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdicionarRefeicao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdicionarRefeicao);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
