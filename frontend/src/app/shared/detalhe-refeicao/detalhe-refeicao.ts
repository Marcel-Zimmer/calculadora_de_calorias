import { Component, EventEmitter, inject, input, Output, signal, effect, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RefeicaoService } from '../../core/services/refeicao.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-detalhe-refeicao',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './detalhe-refeicao.html'
})
export class DetalheRefeicaoComponent {
  private refeicaoService = inject(RefeicaoService);

  refeicao = input.required<any>(); // Refeicao completa
  mostrarModal = input.required<boolean>();
  @Output() mostrarModalChange = new EventEmitter<boolean>();
  @Output() refeicaoAlterada = new EventEmitter<void>();

  novoApelido = signal<string>('');
  editandoApelido = signal<boolean>(false);
  carregando = signal<boolean>(false);

  constructor() {
    effect(() => {
      if (this.mostrarModal() && this.refeicao()) {
        this.novoApelido.set(this.refeicao().apelido || this.refeicao().alimento || 'Sem nome');
        this.editandoApelido.set(false);
      }
    }, { allowSignalWrites: true });
  }

  macros = computed(() => {
    const r = this.refeicao();
    if (!r) return [];
    return [
      { nome: 'Proteínas', valor: r.proteinas, cor: 'bg-rose-500', unidade: 'g' },
      { nome: 'Carbos', valor: r.carboidratos, cor: 'bg-amber-500', unidade: 'g' },
      { nome: 'Gorduras', valor: r.gorduras, cor: 'bg-emerald-500', unidade: 'g' },
      { nome: 'Fibras', valor: r.fibras, cor: 'bg-indigo-500', unidade: 'g' }
    ];
  });

  fechar() {
    this.mostrarModalChange.emit(false);
  }

  salvarApelido() {
    if (!this.novoApelido().trim()) return;
    
    this.carregando.set(true);
    this.refeicaoService.atualizarApelido(this.refeicao().id, this.novoApelido()).subscribe({
      next: () => {
        this.carregando.set(false);
        this.editandoApelido.set(false);
        this.refeicaoAlterada.emit();
        Swal.fire({ icon: 'success', title: 'Apelido atualizado!', timer: 1500, showConfirmButton: false });
      },
      error: () => {
        this.carregando.set(false);
        Swal.fire('Erro', 'Não foi possível atualizar o apelido.', 'error');
      }
    });
  }

  excluir() {
    Swal.fire({
      title: 'Excluir refeição?',
      text: "Esta ação não pode ser desfeita.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#f43f5e',
      confirmButtonText: 'Sim, excluir'
    }).then((result) => {
      if (result.isConfirmed) {
        this.refeicaoService.excluir(this.refeicao().id).subscribe(() => {
          this.refeicaoAlterada.emit();
          this.fechar();
        });
      }
    });
  }
}
