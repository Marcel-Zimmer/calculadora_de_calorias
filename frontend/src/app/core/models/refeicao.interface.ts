export interface RefeicaoModeloResponse {
  id: number;
  apelido?: string;
  calorias?: number;
  proteinas?: number;
  carboidratos?: number;
  gorduras?: number;
  pesoOriginal: number;
}

export interface CriarRefeicaoRequest {
  apelido?: string;
  pesoEmGramas: number;
  tipo: number;
  data: string;
  imagem?: File;
  codigoRefeicaoModelo?: number;
  caloriasManuais?: number;
  alimentoManual?: string;
  proteinasManuais?: number;
  carboidratosManuais?: number;
  gordurasManuais?: number;
  acucaresManuais?: number;
  fibrasManuais?: number;
}
