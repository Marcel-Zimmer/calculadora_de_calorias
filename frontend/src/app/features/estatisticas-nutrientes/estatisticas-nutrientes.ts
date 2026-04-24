import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NutrientesService, NutrientesResponse } from '../../core/services/nutrientes.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';

@Component({
  selector: 'app-estatisticas-nutrientes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './estatisticas-nutrientes.html',
})
export class EstatisticasNutrientesComponent implements OnInit {
  private nutrientesService = inject(NutrientesService);
  private autenticacao = inject(AutenticacaoService);

  periodo = signal<'diario' | 'semanal' | 'mensal'>('diario');
  dados = signal<NutrientesResponse | null>(null);

  ngOnInit(): void {
    this.carregarDados();
  }

  alterarPeriodo(novoPeriodo: 'diario' | 'semanal' | 'mensal') {
    this.periodo.set(novoPeriodo);
    this.carregarDados();
  }

  carregarDados() {
    const userId = this.autenticacao.obterId();
    let obs;

    if (this.periodo() === 'diario') {
      obs = this.nutrientesService.obterNutrientesDiario(userId);
    } else if (this.periodo() === 'semanal') {
      obs = this.nutrientesService.obterNutrientesSemanal(userId);
    } else {
      obs = this.nutrientesService.obterNutrientesMensal(userId);
    }

    obs.subscribe({
      next: (resposta) => {
        this.dados.set(resposta);
      },
      error: (erro) => {
        console.error('Falha ao carregar nutrientes', erro);
      }
    });
  }

  obterPercentual(consumo: number, meta: number): number {
    if (!meta || meta === 0) return 0;
    const perc = (consumo / meta) * 100;
    return perc > 100 ? 100 : perc;
  }
}
