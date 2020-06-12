using System;
using System.Data;
using System.Data.Common;

namespace PA.Services
{
    public abstract class SqlBaseService
    {
        protected static void HandleExecuteNonQuery(IDbCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (DbException ex)
            {
                if (int.TryParse((string)ex.Data["SqlState"], out int sqlState))
                {
                    if (sqlState == 23505)
                    {
                        throw new ProductAlreadyExists();
                    }
                }
                //throw;
            }
        }
    }
}