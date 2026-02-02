

namespace CalculadoraCalorias.Core.Domain.Common
{
    public class Resultado
    {
        public bool Sucesso { get; }
        public TipoDeErro TipoErro { get; }
        public string? MensagemErro { get; }

        protected Resultado(bool sucesso, TipoDeErro tipoErro, string mensagemErro)
        {
            Sucesso = sucesso;
            TipoErro = tipoErro;
            MensagemErro = mensagemErro;
        }
        protected Resultado(bool sucesso)
        {
            Sucesso = sucesso;

        }

        public static Resultado Success()
        {
            return new Resultado(true);
        }

        public static Resultado Failure(TipoDeErro tipoErro, string mensagemErro)
        {
            return new Resultado(false, tipoErro, mensagemErro);
        }
    }
    public class Resultado<T> : Resultado
    {
        public T? Data { get; }
        private Resultado(T? data, bool sucesso, TipoDeErro tipoErro, string mensagemErro)
            : base(sucesso, tipoErro, mensagemErro)
        {
            Data = data;
        }
        private Resultado(T? data, bool sucesso)
            : base(sucesso)
        {
            Data = data;
        }
        public static Resultado<T> Success(T data)
        {
            return new Resultado<T>(data, true, TipoDeErro.None, string.Empty);
        }

        public new static Resultado<T> Failure(TipoDeErro tipoErro, string mensagemErro)
        {
            return new Resultado<T>(default, false, tipoErro, mensagemErro);
        }

        public static implicit operator Resultado<T>(T data) => Success(data);
    }
}
