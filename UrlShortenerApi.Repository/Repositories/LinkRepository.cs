using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SqlClient;
using UrlShortenerApi.Entity.DBEntities;

namespace UrlShortenerApi.Repository.Repositories
{
    public class LinkRepository
    {
        public IEnumerable<LinkEntity> GetByUser(string cs, string username)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                return conn.Query<LinkEntity>("sp_link_GetByUser", new { username }, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }

        public LinkEntity? GetByShortLink(string cs, string shortLink)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                return conn.QueryFirstOrDefault<LinkEntity>("sp_link_GetByShortLink", new { shortLink }, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }

        public LinkEntity? GetById(string cs, Guid id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                return conn.QueryFirstOrDefault<LinkEntity>("sp_link_GetById", new { id }, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }

        public void Insert(string cs, LinkEntity entity)
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

        public void Update(string cs, LinkEntity entity)
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

        public void Delete(string cs, LinkEntity entity)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Delete(entity);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error. Code: {sqlEx.Number}");
            }
        }
    }
}
