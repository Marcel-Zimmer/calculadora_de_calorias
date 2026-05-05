import { RegistroAguaResponse } from './agua.interface';

export interface DadoHistorico {
    legenda: string | number;
    valor: number;
}

export interface RefeicaoDTO {
    id: number;
    apelido?: string;
    calorias: number;
    pesoEmGramas: number;
    data: string;
    tipo: number;
    proteinas?: number;
    carboidratos?: number;
    gorduras?: number;
}

export interface ExercicioDTO {
    id: number;
    tipo: number;
    caloriasEstimadas: number;
    tempoDeExercicio: string;
    dataDoExercicio: string;
}

export interface GraficoDiarioResponse {
    metaCaloricaDiaria: number;
    totalCaloriasConsumidas: number;
    totalCaloriasGastas: number;
    caloriasCalculadas: number;
    totalAguaMl: number;
    refeicoes: RefeicaoDTO[];
    exercicios: ExercicioDTO[];
    registrosAgua: RegistroAguaResponse[];
}

export interface GraficoPontoResponse {
    legenda: string;
    caloriasConsumidas: number;
    caloriasGastas: number;
    saldoCalorico: number;
    aguaMl: number;
    data: string;
}

export interface DashboardInsightsResponse {
    diasNaMeta: number;
    totalDias: number;
    saldoTotal: number;
    impactoPeso: number;
    diferencaAbsoluta: number;
}

export interface GraficoPeriodoResponse {
    metaCaloricaDiaria: number;
    taxaMetabolicaBasal: number;
    totalCaloriasConsumidas: number;
    totalCaloriasGastas: number;
    caloriasCalculadas: number;
    totalAguaMl: number;
    mediaAguaDiaria: number;
    pontos: GraficoPontoResponse[];
    insights?: DashboardInsightsResponse;
}

export interface DistribuicaoItemResponse {
    nome: string;
    displayNome: string;
    icone: string;
    corCss: string;
    valor: number;
    media: number;
    percentual: number;
}

export interface SemanaMediaResponse {
    nome: string;
    valor: number;
}

export interface ConsumoMensalInsightsResponse {
    maiorIngestao: number;
    diaMaiorIngestao: string;
    mediasSemanais: SemanaMediaResponse[];
    topPicos: GraficoPontoResponse[];
}

export interface GastoMensalInsightsResponse {
    maiorGasto: number;
    diaMaiorGasto: string;
    exercicioPrincipal: string;
    mediasSemanais: SemanaMediaResponse[];
    topGasto: GraficoPontoResponse[];
}

export interface EstatisticasDetalhadasResponse {
    metaCaloricaDiaria: number;
    taxaMetabolicaBasal: number;
    totalConsumido: number;
    totalGasto: number;
    mediaConsumoDiario: number;
    mediaGastoDiario: number;
    diasComDados: number;
    pontos: GraficoPontoResponse[];
    distribuicaoExercicios: DistribuicaoItemResponse[];
    distribuicaoRefeicoes: DistribuicaoItemResponse[];
    insights?: DashboardInsightsResponse;
    consumoInsights?: ConsumoMensalInsightsResponse;
    gastoInsights?: GastoMensalInsightsResponse;
}

export interface PesoPontoResponse {
    id: number;
    data: string;
    legenda: string;
    peso: number;
}

export interface EstatisticasPesoResponse {
    pesoAtual: number;
    pesoInicial: number;
    maiorPeso: number;
    menorPeso: number;
    variacaoTotal: number;
    imcAtual: number;
    tmbAtual: number;
    objetivo: string;
    objetivoId: number;
    historico: PesoPontoResponse[];
}
