export enum ObjetivoEnum {
    PerdaPesoAgressiva = 1,
    PerdaPesoLeve = 2,
    ManterPeso = 3
}

export const ObjetivoDescricao: Record<number, string> = {
    [ObjetivoEnum.PerdaPesoAgressiva]: 'Perda de Peso (Foco)',
    [ObjetivoEnum.PerdaPesoLeve]: 'Perda de Peso Leve',
    [ObjetivoEnum.ManterPeso]: 'Manutenção'
};
