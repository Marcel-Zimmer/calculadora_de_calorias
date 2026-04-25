import { Component, inject, OnInit, signal, computed } from '@angular/core';
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
    if (!d) return null;

    const items = [
      { nome: 'Proteína', valor: d.consumoProteinas, meta: d.metaProteinas, icone: '🥩', cor: 'stroke-rose-500', bg: 'text-rose-100' },
      { nome: 'Carbo', valor: d.consumoCarboidratos, meta: d.metaCarboidratos, icone: '🍞', cor: 'stroke-amber-400', bg: 'text-amber-100' },
      { nome: 'Gordura', valor: d.consumoGorduras, meta: d.metaGorduras, icone: '🥑', cor: 'stroke-indigo-500', bg: 'text-indigo-100' },
      { nome: 'Fibras', valor: d.consumoFibras, meta: d.metaFibras, icone: '🥦', cor: 'stroke-emerald-500', bg: 'text-emerald-100' },
      { nome: 'Açúcar', valor: d.consumoAcucares, meta: d.limiteAcucares, icone: '🍭', 
        cor: d.consumoAcucares > d.limiteAcucares ? 'stroke-rose-600' : 'stroke-sky-400', 
        bg: d.consumoAcucares > d.limiteAcucares ? 'text-rose-200' : 'text-sky-100',
        isLimite: true
      },
    ];

    return items.map(item => ({
      ...item,
      percentual: this.obterPercentual(item.valor, item.meta),
      strokeDasharray: `${this.obterPercentual(item.valor, item.meta)} 100`
    }));
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
