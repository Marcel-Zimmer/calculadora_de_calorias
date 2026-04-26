import { Component, inject, OnInit, signal, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GraficoService } from '../../../core/services/grafico.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import { BsCardMaiorGastoComponent } from '../../../shared/bs-card-maior-gasto/bs-card-maior-gasto';
import { BsGraficoMediaPorSemanaComponent } from '../../../shared/bs-grafico-media-por-semana/bs-grafico-media-por-semana';
import { BsGraficoMaioresConsumosComponent } from '../../../shared/bs-grafico-maiores-consumos/bs-grafico-maiores-consumos';
import { BsGraficoMediaSemanalComponent } from '../../../shared/bs-grafico-media-semanal/bs-grafico-media-semanal';
import { BsGraficoHistoricoMensalComponent, DadoHistorico } from '../../../shared/bs-grafico-historico-mensal/bs-grafico-historico-mensal';
import { BsGraficoDistribuicaoItensComponent } from '../../../shared/bs-grafico-distribuicao-itens/bs-grafico-distribuicao-itens';

@Component({
  selector: 'app-gasto-calorico',
  standalone: true,
  imports: [
    CommonModule, BsCardMaiorGastoComponent, BsGraficoMediaPorSemanaComponent, 
    BsGraficoMaioresConsumosComponent, BsGraficoMediaSemanalComponent, 
    BsGraficoHistoricoMensalComponent, BsGraficoDistribuicaoItensComponent
  ],
  templateUrl: './gasto-calorico.html'
})
export class GastoCaloricoComponent implements OnInit {
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
      if (res.distribuicaoExercicios) {
        res.distribuicaoExercicios = res.distribuicaoExercicios.map((item: any) => {
          const icones: any = { '1': '🚴', '2': '🚶', '3': '🏃', '4': '⚽', '5': '🏋️', '6': '🏊', '7': '💪' };
          const cores: any = { '1': 'bg-blue-50 text-blue-500', '2': 'bg-emerald-50 text-emerald-500', '3': 'bg-orange-50 text-orange-500', '4': 'bg-green-50 text-green-500', '5': 'bg-indigo-50 text-indigo-500', '6': 'bg-cyan-50 text-cyan-500', '7': 'bg-rose-50 text-rose-500' };
          const nomes: any = { '1': 'Ciclismo', '2': 'Caminhada', '3': 'Corrida', '4': 'Futebol', '5': 'Musculação', '6': 'Natação', '7': 'Crossfit' };
          return { ...item, displayNome: nomes[item.nome] || 'Outro', icone: icones[item.nome] || '🔥', corCss: cores[item.nome] || 'bg-slate-50 text-slate-500' };
        });
      }

      if (res.gastoInsights?.topGasto) {
        res.gastoInsights.topGasto = res.gastoInsights.topGasto.map((p: any) => ({
          legenda: p.legenda,
          valor: p.caloriasGastas
        }));
      }

      this.dadosEstatisticas.set(res);
    });
  }

  alterarPeriodo(p: 'semanal' | 'mensal') { this.periodoEstatisticas.set(p); }

  dadosHistoricoGasto = computed<DadoHistorico[]>(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    return dados.pontos.map((p: any) => ({ legenda: p.legenda, valor: p.caloriasGastas }));
  });

  insightsGasto = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados || !dados.pontos || dados.pontos.length === 0) return null;
    const pontos = dados.pontos;
    const diasAtivos = pontos.filter((p: any) => p.caloriasGastas > 0).length;
    const maiorGasto = Math.max(...pontos.map((p: any) => p.caloriasGastas));
    const diaMaiorGasto = pontos.find((p: any) => p.caloriasGastas === maiorGasto)?.legenda || '-';
    return { diasAtivos, maiorGasto, diaMaiorGasto };
  });
}
