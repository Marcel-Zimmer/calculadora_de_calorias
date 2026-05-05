import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GraficoService } from '../../../core/services/grafico.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import { BsCardMediaItemComponent } from '../../../shared/bs-card-media-item/bs-card-media-item';
import { BsGraficoPesoCriativoComponent } from '../../../shared/bs-grafico-peso-criativo/bs-grafico-peso-criativo';
import { AdicionarPesoComponent } from '../../../shared/adicionar-peso/adicionar-peso';
import { AtualizarMetaComponent } from '../../../shared/atualizar-meta/atualizar-meta';
import { EditarPesoComponent } from '../../../shared/editar-peso/editar-peso';
import { ObjetivoEnum, ObjetivoDescricao } from '../../../core/enums/objetivo.enum';

@Component({
  selector: 'app-acompanhamento-peso',
  standalone: true,
  imports: [
    CommonModule, 
    BsCardMediaItemComponent, 
    BsGraficoPesoCriativoComponent,
    AdicionarPesoComponent,
    AtualizarMetaComponent,
    EditarPesoComponent
  ],
  templateUrl: './acompanhamento-peso.html'
})
export class AcompanhamentoPesoComponent implements OnInit {
  private graficoService = inject(GraficoService);
  private autenticacao = inject(AutenticacaoService);

  dadosPeso = signal<any>(null);
  mostrarModalAdicionar = signal<boolean>(false);
  mostrarModalMeta = signal<boolean>(false);
  
  mostrarModalEditar = signal<boolean>(false);
  pontoSelecionado = signal<any>(null);

  ngOnInit(): void {
    this.carregarEstatisticas();
  }

  carregarEstatisticas() {
    this.graficoService.obterEstatisticasPeso().subscribe((res: any) => {
      this.dadosPeso.set(res);
    });
  }

  abrirEditar(ponto: any) {
    this.pontoSelecionado.set(ponto);
    this.mostrarModalEditar.set(true);
  }

  cardsPeso = computed(() => {
    const dados = this.dadosPeso();
    if (!dados) return [];
    
    const objId = dados.objetivoId || dados.ObjetivoId;
    const objDesc = dados.objetivo || dados.Objetivo || ObjetivoDescricao[objId as ObjetivoEnum];
    
    const iconeMeta = objId === ObjetivoEnum.ManterPeso ? '⚖️' : (objId === ObjetivoEnum.PerdaPesoAgressiva ? '🔥' : '🍃');
    const corMeta = objId === ObjetivoEnum.ManterPeso ? 'bg-emerald-50 text-emerald-600' : (objId === ObjetivoEnum.PerdaPesoAgressiva ? 'bg-rose-50 text-rose-600' : 'bg-orange-50 text-orange-600');

    return [
      { icone: iconeMeta, corCss: corMeta, titulo: 'Meta Ativa', valor: objDesc || 'Não Definido', isText: true, legenda: 'Objetivo' },
      { icone: '📈', corCss: 'bg-blue-50 text-blue-600', titulo: 'Variação', valor: dados.variacaoTotal ?? dados.VariacaoTotal, legenda: 'Total kg' },
      { icone: '📏', corCss: 'bg-indigo-50 text-indigo-600', titulo: 'IMC Atual', valor: dados.imcAtual ?? dados.ImcAtual, legenda: 'Massa Corp.' },
      { icone: '⚡', corCss: 'bg-amber-50 text-amber-600', titulo: 'TMB', valor: dados.tmbAtual ?? dados.TmbAtual, legenda: 'kcal/dia' }
    ];
  });

  historicoParaGrafico = computed(() => {
    const dados = this.dadosPeso();
    if (!dados) return [];
    const hist = dados.historico || dados.Historico || [];
    return hist.slice(-7);
  });
}
