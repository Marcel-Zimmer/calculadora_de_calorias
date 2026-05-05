export interface CriarRegistroFisicoRequest {
  pesoKg: number;
  metaCaloricaDiaria?: number;
}

export interface RegistroFisicoResponse {
  id: number;
  usuarioId: number;
  perfilBiometricoId: number;
  imcCalculado: number;
  taxaMetabolicaBasal: number;
  pesoKg: number;
  metaCaloricaDiaria?: number;
}
