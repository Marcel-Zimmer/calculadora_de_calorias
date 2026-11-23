export interface RegistroAguaRequest {
  quantidadeMl: number;
  data?: string;
  hora?: string;
}

export interface RegistroAguaResponse {
  id: number;
  quantidadeMl: number;
  data: string;
  hora: string;
}

export interface AguaPontoResponse {
  legenda: string;
  quantidadeMl: number;
  data: string;
}

export interface EstatisticasAguaResponse {
  metaAguaDiaria: number;
  totalConsumido: number;
  mediaDiaria: number;
  diasNaMeta: number;
  totalDias: number;
  maiorConsumo: number;
  diaMaiorConsumo: string;
  pontos: AguaPontoResponse[];
}
