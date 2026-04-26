import { Component, inject, OnInit, signal, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GraficoService } from '../../../core/services/grafico.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import { BsCardMediaItemComponent } from '../../../shared/bs-card-media-item/bs-card-media-item';
import { BsCardMaiorIngestaoComponent } from '../../../shared/bs-card-maior-ingestao/bs-card-maior-ingestao';
import { BsGraficoMediaPorSemanaComponent } from '../../../shared/bs-grafico-media-por-semana/bs-grafico-media-por-semana';
import { BsGraficoMaioresConsumosComponent } from '../../../shared/bs-grafico-maiores-consumos/bs-grafico-maiores-consumos';
import { BsGraficoMediaSemanalComponent } from '../../../shared/bs-grafico-media-semanal/bs-grafico-media-semanal';
import { BsGraficoHistoricoMensalComponent, DadoHistorico } from '../../../shared/bs-grafico-historico-mensal/bs-grafico-historico-mensal';
import { BsGraficoDistribuicaoItensComponent } from '../../../shared/bs-grafico-distribuicao-itens/bs-grafico-distribuicao-itens';

@Component({
  selector: 'app-consumo-calorico',
  standalone: true,
  imports: [
    CommonModule, BsCardMediaItemComponent, BsCardMaiorIngestaoComponent, 
    BsGraficoMediaPorSemanaComponent, BsGraficoMaioresConsumosComponent,
    BsGraficoMediaSemanalComponent, BsGraficoHistoricoMensalComponent,
    BsGraficoDistribuicaoItensComponent
  ],
  templateUrl: './consumo-calorico.html'
})
export class ConsumoCaloricoComponent implements OnInit {
  private graficoService = inject(GraficoService);
  private autenticacao = inject(AutenticacaoService);

  dataSelecionada = input.required<string>();
  periodoEstatisticas = signal<'semanal' | 'mensal'>('semanal');
  dadosEstatisticas = signal<any>(null);

  constructor() {
    effect(() => {
      this.dataSelecionada();
      this.periodoEstatisticas();
      this.carregarEstatisticas();
    }, { allowSignalWrites: true });
  }

  ngOnInit(): void {}

  carregarEstatisticas() {
    const userId = this.autenticacao.obterId();
    const obs = this.periodoEstatisticas() === 'semanal' 
      ? this.graficoService.obterEstatisticasSemanais(userId, this.dataSelecionada())
      : this.graficoService.obterEstatisticasMensais(userId, this.dataSelecionada());

    obs.subscribe((res: any) => {
      if (res.distribuicaoRefeicoes) {
        res.distribuicaoRefeicoes = res.distribuicaoRefeicoes.map((item: any) => {
          // Mapeamento simplificado para exemplo, idealmente viria de um serviço ou enum
          const icones: any = { '1': '☕', '2': '🍲', '3': '🍽️', '4': '🥪', '5': '🍰' };
          const cores: any = { '1': 'bg-amber-50 text-amber-500', '2': 'bg-emerald-50 text-emerald-500', '3': 'bg-blue-50 text-blue-500', '4': 'bg-indigo-50 text-indigo-500', '5': 'bg-rose-50 text-rose-500' };
          const nomes: any = { '1': 'Café', '2': 'Almoço', '3': 'Jantar', '4': 'Lanche', '5': 'Gula' };
          return { ...item, displayNome: nomes[item.nome] || 'Outro', icone: icones[item.nome] || '🍽️', corCss: cores[item.nome] || 'bg-slate-50 text-slate-500' };
        });
      }

      if (res.consumoInsights?.topPicos) {
        res.consumoInsights.topPicos = res.consumoInsights.topPicos.map((p: any) => ({
          legenda: p.legenda,
          valor: p.caloriasConsumidas
        }));
      }

      this.dadosEstatisticas.set(res);
    });
  }

  alterarPeriodo(p: 'semanal' | 'mensal') { this.periodoEstatisticas.set(p); }

  dadosHistoricoConsumo = computed<DadoHistorico[]>(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    return dados.pontos.map((p: any) => ({ legenda: p.legenda, valor: p.caloriasConsumidas }));
  });

  cardsMediaConsumo = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    const lista = [];
    lista.push({ icone: '📊', corCss: 'bg-slate-50 text-indigo-500', titulo: 'Média Diária', valor: dados.mediaConsumoDiario || 0, legenda: 'no período' });
    (dados.distribuicaoRefeicoes || []).forEach((refeicao: any) => {
        lista.push({ icone: refeicao.icone, corCss: refeicao.corCss, titulo: refeicao.displayNome, valor: refeicao.media || 0, legenda: 'média diária' });
    });
    return lista;
  });
}
