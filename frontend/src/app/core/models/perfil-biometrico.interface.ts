export interface CriarPerfilBiometricoRequest {
  dataNascimento: string;
  genero: number;
  alturaCm: number;
  nivelAtividade: number;
  objetivo: number;
}

export interface PerfilBiometricoResponse {
  id: number;
  usuarioId: number;
  dataNascimento: string;
  genero: number;
  alturaCm: number;
  nivelAtividade: number;
  objetivo: number;
}
