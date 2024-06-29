using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SqlClient;
using UrlShortenerApi.Entity.DBEntities;

namespace UrlShortenerApi.Repository.Repositories
{
    public class UserRepository
    {
        public UserEntity? GetById(string cs, string username)
        {
			try
			{
                SqlConnection conn = new SqlConnection(cs);
                return conn.QueryFirstOrDefault<UserEntity>("sp_user_GetById", new { username }, commandType: CommandType.StoredProcedure);
			}
			catch (SqlException sqlEx)
			{
                throw new Exception($"Database error. Code: {sqlEx.Number}");
			}
        }

        public Guid Login(string cs, string username, string pwd)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                return conn.QueryFirstOrDefault<Guid>("sp_user_Login", new { username, pwd }, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }

        public void Insert(string cs, UserEntity entity)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Insert(entity);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }

        public void Update(string cs, UserEntity entity)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Update(entity);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }
    }
}
