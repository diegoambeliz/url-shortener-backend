using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortenerApi.Dto;
using UrlShortenerApi.Entity.DBEntities;
using UrlShortenerApi.Service.Services;
using UrlShortenerApi.Util;

namespace UrlShortenerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly LinkService _service;
        protected readonly IMapper _mapper;

        public LinkController(IConfiguration config, LinkService service, IMapper mapper, IMailer mailer)
        {
            _config = config;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetOwnList")]
        [Authorize]
        public ActionResult GetByUser()
        {

            try
            {
                var cs = _config["CS"];
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var username = identity.Claims.Where(x => x.Type == "User").FirstOrDefault()?.Value;
                var entities = _service.GetByUser(cs, username);

                var dto = _mapper.Map<IEnumerable<LinkViewDto>>(entities);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{shortCodeLink}")]
        [Authorize]
        public ActionResult GetByUser(string shortCodeLink)
        {

            try
            {
                var cs = _config["CS"];
                var link = _service.RedirectToLink(cs, shortCodeLink);
                return Ok(link);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Insert(LinkCreateDto dto)
        {

            try
            {
                var cs = _config["CS"];
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var username = identity.Claims.Where(x => x.Type == "User").FirstOrDefault()?.Value;
                var entity = _mapper.Map<LinkEntity>(dto);

                entity.createdBy = username;
                entity.createdAt = DateTime.Now;
                entity.id = Guid.NewGuid();

                var shortLink = _service.Insert(cs, entity);

                return Ok(shortLink);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(Guid id)
        {

            try
            {
                var cs = _config["CS"];

                _service.Delete(cs, id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
