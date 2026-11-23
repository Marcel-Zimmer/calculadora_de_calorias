export interface Usuario {
    id: number;
    nome?: string;
    email?: string;
    role?: number;
}

export interface LoginUsuarioResponse {
    id: number;
    nome?: string;
    email?: string;
    role: number;
    accessToken?: string;
    refreshToken?: string;
}

export interface UsuarioLogin{
    email:string;
    senha:string;
}

export interface UsuarioRegistro {
    nome: string;
    email: string;
    senha: string;
    dataNascimento: string;
    genero: number;
    alturaCm: number;
    pesoKg: number;
    nivelAtividade: number;
    objetivo: number;
    metaCaloricaDiaria?: number | null;
}