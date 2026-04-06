import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdicionarExercicio } from './adicionar-exercicio';

describe('AdicionarExercicio', () => {
  let component: AdicionarExercicio;
  let fixture: ComponentFixture<AdicionarExercicio>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdicionarExercicio]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdicionarExercicio);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
