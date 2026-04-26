import { Component, inject, OnInit, signal, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NutrientesService, NutrientesResponse } from '../../../core/services/nutrientes.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import { BsCardNutrientesComponent } from '../../../shared/bs-card-nutrientes/bs-card-nutrientes';
import { NutrientesEnum } from '../../../core/models/nutrientes.enum';
import { BsGraficoDivisaoCaloriasComponent } from '../../../shared/bs-grafico-divisao-calorias/bs-grafico-divisao-calorias';
import { BsCardPerfilAnabolicoComponent } from '../../../shared/bs-card-perfil-anabolico/bs-card-perfil-anabolico';

@Component({
  selector: 'app-nutrientes',
  standalone: true,
  imports: [CommonModule, BsCardNutrientesComponent, BsGraficoDivisaoCaloriasComponent, BsCardPerfilAnabolicoComponent],
  templateUrl: './nutrientes.html',
})
export class NutrientesComponent implements OnInit {
  private nutrientesService = inject(NutrientesService);
  private autenticacao = inject(AutenticacaoService);

  dataSelecionada = input.required<string>();
  periodo = signal<'diario' | 'semanal' | 'mensal'>('diario');
  dados = signal<NutrientesResponse | null>(null);

  mapaNutrientes = this.nutrientesService.obterMapaNutrientes();

  constructor() {
    effect(() => {
      this.dataSelecionada();
      this.periodo();
      this.carregarDados();
    }, { allowSignalWrites: true });
  }

  progressoNutrientes = computed(() => {
    const d = this.dados();
    if (!d || !d.detalhes) return [];

    return d.detalhes.map((item: any) => {
        const info = this.mapaNutrientes[item.tipo];
        return {
            ...info,
            valor: item.valor,
            meta: item.meta,
            cor: (item.tipo === NutrientesEnum.Acucar && item.valor > item.meta) ? 'stroke-rose-600' : info.cor,
            bg: (item.tipo === NutrientesEnum.Acucar && item.valor > item.meta) ? 'text-rose-200' : info.bg
        };
    });
  });

  ngOnInit(): void {}

  alterarPeriodo(novoPeriodo: 'diario' | 'semanal' | 'mensal') {
    this.periodo.set(novoPeriodo);
  }

  carregarDados() {
    const userId = this.autenticacao.obterId();
    const data = this.dataSelecionada();
    let obs;

    if (this.periodo() === 'diario') {
      obs = this.nutrientesService.obterNutrientesDiario(userId, data);
    } else if (this.periodo() === 'semanal') {
      obs = this.nutrientesService.obterNutrientesSemanal(userId, data);
    } else {
      obs = this.nutrientesService.obterNutrientesMensal(userId, data);
    }

    obs.subscribe({
      next: (resposta: any) => {
        this.dados.set(resposta);
      },
      error: (erro: any) => {
        console.error('Falha ao carregar nutrientes', erro);
      }
    });
  }
}
