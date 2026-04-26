import { Component, inject, OnInit, signal, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NutrientesService, NutrientesResponse } from '../../core/services/nutrientes.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { BsCardNutrientesComponent } from '../../shared/bs-card-nutrientes/bs-card-nutrientes';
import { NutrientesEnum } from '../../core/models/nutrientes.enum';

@Component({
  selector: 'app-estatisticas-nutrientes',
  standalone: true,
  imports: [CommonModule, BsCardNutrientesComponent],
  templateUrl: './estatisticas-nutrientes.html',
})
export class EstatisticasNutrientesComponent implements OnInit {
  private nutrientesService = inject(NutrientesService);
  private autenticacao = inject(AutenticacaoService);

  dataSelecionada = input<string>(new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' }));
  periodo = signal<'diario' | 'semanal' | 'mensal'>('diario');
  dados = signal<NutrientesResponse | null>(null);

  mapaNutrientes = this.nutrientesService.obterMapaNutrientes();

  constructor() {
    effect(() => {
      // Reage a mudanças na data selecionada ou no período
      this.dataSelecionada();
      this.periodo();
      this.carregarDados();
    }, { allowSignalWrites: true });
  }

  // --- CÁLCULOS AVANÇADOS (COMPUTED SIGNALS) ---

  // 1. Distribuição Calórica dos Macronutrientes
  distribuicaoMacros = computed(() => {
    const d = this.dados();
    if (!d) return null;

    const calProt = (d.consumoProteinas || 0) * 4;
    const calCarb = (d.consumoCarboidratos || 0) * 4;
    const calGord = (d.consumoGorduras || 0) * 9;
    const total = calProt + calCarb + calGord || 1;

    const percProt = (calProt / total) * 100;
    const percCarb = (calCarb / total) * 100;
    const percGord = (calGord / total) * 100;

    // Lógica para o Donut SVG (Circunferência = 2 * PI * r = 2 * 3.14 * 15.915 ≈ 100)
    // Usamos r=15.915 para que a circunferência seja exatamente 100, facilitando o uso de porcentagens.
    const offsetProt = 0;
    const offsetCarb = percProt;
    const offsetGord = percProt + percCarb;

    return { 
      calProt, calCarb, calGord, total,
      percProt, percCarb, percGord,
      offsetProt, offsetCarb, offsetGord
    };
  });

  // 2. Metas Individuais (Anéis)
  progressoNutrientes = computed(() => {
    const d = this.dados();
    if (!d || !d.detalhes) return [];

    return d.detalhes.map(item => {
        const info = this.mapaNutrientes[item.tipo];
        return {
            ...info,
            valor: item.valor,
            meta: item.meta,
            // Sobrescreve cor se for açúcar e estourou
            cor: (item.tipo === NutrientesEnum.Acucar && item.valor > item.meta) ? 'stroke-rose-600' : info.cor,
            bg: (item.tipo === NutrientesEnum.Acucar && item.valor > item.meta) ? 'text-rose-200' : info.bg
        };
    });
  });

  // 3. Análise de Perfil Nutricional (Veredito Inteligente)
  perfilNutricional = computed(() => {
    const macros = this.distribuicaoMacros();
    if (!macros || macros.total < 100) return null; // Só analisa se houver consumo relevante

    const { percProt, percCarb, percGord } = macros;

    if (percProt > 25) {
      return { 
        titulo: 'Perfil Anabólico', 
        descricao: 'Sua dieta está priorizando a regeneração e construção muscular.', 
        cor: 'text-rose-600 bg-rose-50',
        icone: '💪' 
      };
    }
    
    if (percGord > 35) {
      return { 
        titulo: 'Perfil Lipídico', 
        descricao: 'Consumo elevado de gorduras. Atente-se à qualidade das fontes.', 
        cor: 'text-orange-600 bg-orange-50',
        icone: '🥑' 
      };
    }

    if (percCarb > 60) {
      return { 
        titulo: 'Perfil Energético', 
        descricao: 'Foco alto em carboidratos, ideal para dias de treinos intensos.', 
        cor: 'text-amber-600 bg-amber-50',
        icone: '⚡' 
      };
    }

    return { 
      titulo: 'Perfil Equilibrado', 
      descricao: 'Excelente harmonia entre proteínas, carbos e gorduras.', 
      cor: 'text-emerald-600 bg-emerald-50',
      icone: '⚖️' 
    };
  });

  ngOnInit(): void {
  }

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
