using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Repositories
{
    public class BaseRepository
    {
        private readonly DatabaseOptions _options;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public BaseRepository()
        {
        }

        public BaseRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _options = databaseOptions.Value;
            _connection = new SqlConnection(_options.ConnectionString);
        }
        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }
    }
}
