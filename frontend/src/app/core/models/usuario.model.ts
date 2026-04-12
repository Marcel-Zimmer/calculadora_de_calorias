export interface Usuario {
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