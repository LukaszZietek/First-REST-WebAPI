using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Entities;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/meetup/{meetupName}/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly MeetupContext _meetupContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LectureController> _logger;

        public LectureController(MeetupContext meetupContext, IMapper mapper, ILogger<LectureController> logger)
        {
            _meetupContext = meetupContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get(string meetupName)
        {
            var meetup = _meetupContext.Meetups
                .Include(x => x.Lectures)
                .FirstOrDefault(l => l.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var lectures = _mapper.Map<List<LectureDto>>(meetup.Lectures);

            return Ok(lectures);
        }

        [HttpPost]
        public ActionResult Post(string meetupName, [FromBody] LectureDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetup = _meetupContext.Meetups
                .Include(x => x.Lectures)
                .FirstOrDefault(l => l.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var lecture = _mapper.Map<Lecture>(model);
            meetup.Lectures.Add(lecture);
            _meetupContext.SaveChanges();

            return Created($"api/meetup/{meetupName}", null);

        }

        [HttpDelete]
        public ActionResult Delete(string meetupName)
        {

            var meetup = _meetupContext.Meetups
                .Include(x => x.Lectures)
                .FirstOrDefault(l => l.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());
            if (meetup == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"Wyklady dla meetupu {meetupName} zostały usunięte");

            _meetupContext.Lectures.RemoveRange(meetup.Lectures);
            _meetupContext.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string meetupName, int id)
        {
            var meetup = _meetupContext.Meetups
                .Include(x => x.Lectures)
                .FirstOrDefault(l => l.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());
            if (meetup == null)
            {
                return NotFound();
            }

            var lecture = meetup.Lectures.FirstOrDefault(x => x.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }

            _meetupContext.Lectures.Remove(lecture);
            _meetupContext.SaveChanges();

            return NoContent();
        }

    }
}
