import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastrarRefeicao } from './cadastrar-refeicao';

describe('CadastrarRefeicao', () => {
  let component: CadastrarRefeicao;
  let fixture: ComponentFixture<CadastrarRefeicao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastrarRefeicao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastrarRefeicao);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
