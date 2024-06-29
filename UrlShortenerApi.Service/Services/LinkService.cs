using System.Text;
using System;
using UrlShortenerApi.Entity.DBEntities;
using UrlShortenerApi.Repository.Repositories;

namespace UrlShortenerApi.Service.Services
{
    public class LinkService
    {

        protected const int LENGHT = 10;

        protected readonly LinkRepository _repo;
        public LinkService(LinkRepository repo) => _repo = repo;
        public IEnumerable<LinkEntity> GetByUser(string cs, string username) => _repo.GetByUser(cs, username);
        public LinkEntity? GetById(string cs, Guid id) => _repo.GetById(cs, id);
        public string Insert(string cs, LinkEntity entity)
        {
            var shortLink = GenerateShortLink();

            entity.shortLink = shortLink;
            entity.viewsCount = 0;

            _repo.Insert(cs, entity);
            return shortLink;
        }
        public void Delete(string cs, Guid id)
        {
            var linkEntity = GetById(cs, id);

            if (linkEntity == null)
                throw new ArgumentException("Invalid link, not found.");

            _repo.Delete(cs, linkEntity);
        }
        public string RedirectToLink(string cs, string shortLink)
        {
            var linkEntity = _repo.GetByShortLink(cs, shortLink);

            if (linkEntity == null)
                throw new ArgumentException("Invalid link, not found.");

            linkEntity.viewsCount++;

            _repo.Update(cs, linkEntity);

            return linkEntity.link;
        }

        private static string GenerateShortLink()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringBuilder = new StringBuilder(LENGHT);

            for (int i = 0; i < LENGHT; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
