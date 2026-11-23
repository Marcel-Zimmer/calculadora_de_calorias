export interface AtividadeFisicaResponse {
  id?: number;
  usuarioId?: number;
  tipoAtividadeId?: number;
  caloriasEstimadas?: number;
  dataExercicio: string;
  tempoExercicio: string;
}

export interface CriarAtividadeFisicaRequest {
  caloriasEstimadas: number;
  tipo: number;
  tempoDeExercicio: string;
  dataDoExercicio: string;
}
