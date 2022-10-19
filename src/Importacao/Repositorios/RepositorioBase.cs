using FirebirdSql.Data.FirebirdClient;
using System;

namespace Importacao.Repositorios
{
    public class RepositorioBase : IDisposable
    {
        protected FbConnection Connection { get; set; }
        protected FbTransaction Transaction { get; set; }
        public RepositorioBase(FbConnection connection)
        {
            Connection = connection;
            CriaInsCmd();
        }

        public void SetTransaction(FbTransaction transaction)
        {
            Transaction = transaction;
            InsCmd.Transaction = Transaction;
        }

        protected FbCommand InsCmd;

        protected virtual void CriaInsCmd()
        {
        }

        protected static object ValorOuNulo(string valor)
        {
            if (valor == null)
                return DBNull.Value;
            else
                return valor;
        }

        protected static object ValorOuNulo(int? valor)
        {
            if (valor.HasValue)
                return valor.Value;
            else
                return DBNull.Value;
        }

        protected static object ValorOuNulo(DateTime? valor)
        {
            if (valor.HasValue)
                return valor.Value;
            else
                return DBNull.Value;
        }

        protected static object ValorOuNulo(Decimal? valor)
        {
            if (valor.HasValue)
                return valor.Value;
            else
                return DBNull.Value;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    InsCmd.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
