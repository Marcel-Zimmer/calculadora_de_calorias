import { Component, inject, OnInit, signal, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AguaService } from '../../../core/services/agua.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import { BsCardMediaItemComponent } from '../../../shared/bs-card-media-item/bs-card-media-item';
import { BsCardMaiorIngestaoComponent } from '../../../shared/bs-card-maior-ingestao/bs-card-maior-ingestao';
import { BsGraficoMediaSemanalComponent } from '../../../shared/bs-grafico-media-semanal/bs-grafico-media-semanal';
import { BsGraficoHistoricoMensalComponent, DadoHistorico } from '../../../shared/bs-grafico-historico-mensal/bs-grafico-historico-mensal';
import { BsCardConsistenciaComponent } from '../../../shared/bs-card-consistencia/bs-card-consistencia';

@Component({
  selector: 'app-consumo-agua',
  standalone: true,
  imports: [
    CommonModule, BsCardMediaItemComponent, BsCardMaiorIngestaoComponent, 
    BsGraficoMediaSemanalComponent, BsGraficoHistoricoMensalComponent,
    BsCardConsistenciaComponent
  ],
  templateUrl: './consumo-agua.html'
})
export class ConsumoAguaComponent implements OnInit {
  private aguaService = inject(AguaService);
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
    const obs = this.periodoEstatisticas() === 'semanal' 
      ? this.aguaService.obterEstatisticasSemanais(this.dataSelecionada())
      : this.aguaService.obterEstatisticasMensais(this.dataSelecionada());

    obs.subscribe((res: any) => {
      this.dadosEstatisticas.set(res);
    });
  }

  alterarPeriodo(p: 'semanal' | 'mensal') { this.periodoEstatisticas.set(p); }

  dadosHistoricoConsumo = computed<DadoHistorico[]>(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    return dados.pontos.map((p: any) => ({ legenda: p.legenda, valor: p.quantidadeMl }));
  });

  cardsMediaConsumo = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    
    return [
      { icone: '🎯', corCss: 'bg-blue-50 text-blue-600', titulo: 'Meta Diária', valor: `${dados.metaAguaDiaria}ml`, legenda: 'baseado no peso', isText: true },
      { icone: '📊', corCss: 'bg-blue-50 text-blue-500', titulo: 'Média Diária', valor: `${dados.mediaDiaria}ml`, legenda: 'no período', isText: true },
      { icone: '💧', corCss: 'bg-blue-50 text-blue-700', titulo: 'Total Consumido', valor: `${(dados.totalConsumido / 1000).toFixed(1)}L`, legenda: 'no período', isText: true }
    ];
  });
}
