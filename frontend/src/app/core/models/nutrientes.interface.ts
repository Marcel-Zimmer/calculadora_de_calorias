export interface NutrienteDetalhe {
  tipo: number;
  nome: string;
  valor: number;
  meta: number;
  isLimite: boolean;
}

export interface MacrosResponse {
  caloriasProteinas: number;
  caloriasCarboidratos: number;
  caloriasGorduras: number;
  totalCalorias: number;
  percentualProteinas: number;
  percentualCarboidratos: number;
  percentualGorduras: number;
}

export interface VereditoResponse {
  titulo: string;
  descricao: string;
  icone: string;
  corCss: string;
}

export interface NutrientesResponse {
  periodo: string;
  metaProteinas: number;
  consumoProteinas: number;
  metaCarboidratos: number;
  consumoCarboidratos: number;
  metaGorduras: number;
  consumoGorduras: number;
  metaFibras: number;
  consumoFibras: number;
  limiteAcucares: number;
  consumoAcucares: number;
  detalhes: NutrienteDetalhe[];
  macros?: MacrosResponse;
  veredito?: VereditoResponse;
}
