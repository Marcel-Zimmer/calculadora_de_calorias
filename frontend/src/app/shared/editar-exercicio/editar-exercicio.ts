import { Component, EventEmitter, inject, input, Output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AtividadeFisicaService } from '../../core/services/atividade-fisica.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-editar-exercicio',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './editar-exercicio.html'
})
export class EditarExercicioComponent {
  private atividadeService = inject(AtividadeFisicaService);

  exercicio = input.required<any>();
  mostrarModal = input.required<boolean>();
  @Output() mostrarModalChange = new EventEmitter<boolean>();
  @Output() exercicioAlterado = new EventEmitter<void>();

  descricao = signal<string>('');
  duracao = signal<number>(0);
  carregando = signal<boolean>(false);

  mapaExercicios = this.atividadeService.obterMapaExercicios();

  constructor() {
    effect(() => {
      if (this.mostrarModal() && this.exercicio()) {
        this.descricao.set(this.exercicio().descricao);
        this.duracao.set(this.exercicio().duracaoMinutos);
      }
    }, { allowSignalWrites: true });
  }

  fechar() {
    this.mostrarModalChange.emit(false);
  }

  salvar() {
    if (!this.descricao() || this.duracao() <= 0) return;

    this.carregando.set(true);
    const payload = {
      id: this.exercicio().id,
      usuarioId: this.exercicio().usuarioId,
      descricao: this.descricao(),
      duracaoMinutos: this.duracao(),
      tipoExercicio: this.exercicio().tipoExercicio,
      data: this.exercicio().data
    };

    this.atividadeService.atualizar(payload).subscribe({
      next: () => {
        this.carregando.set(false);
        this.exercicioAlterado.emit();
        this.fechar();
        Swal.fire({ icon: 'success', title: 'Exercício atualizado!', timer: 1500, showConfirmButton: false });
      },
      error: () => {
        this.carregando.set(false);
        Swal.fire('Erro', 'Não foi possível atualizar.', 'error');
      }
    });
  }

  excluir() {
    Swal.fire({
      title: 'Excluir exercício?',
      text: "Esta ação não pode ser desfeita.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#f43f5e',
      confirmButtonText: 'Sim, excluir'
    }).then((result) => {
      if (result.isConfirmed) {
        this.atividadeService.excluir(this.exercicio().id).subscribe(() => {
          this.exercicioAlterado.emit();
          this.fechar();
        });
      }
    });
  }
}
