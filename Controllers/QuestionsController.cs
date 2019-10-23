using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using assess.Models;
using assess.Services;
using assess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace assess.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionService _service;

        public QuestionsController(QuestionService service)
        {
            _service = service;
        }

        [HttpGet("{id:length(24)}")]
        public ActionResult<Question> GetQuestionsById(string id) => _service.Get(id);

        [HttpGet]
        public ActionResult<List<Question>> GetQuestionsByTypeOrCategory([FromQuery] string type = null, [FromQuery] string category = null)
        {
            if (type != null && category != null)
            {
                return BadRequest("Both not nulls");
            }

            if (type == null && category == null)
            {
                return _service.Get();
            }

            if (type != null)
            {
                return _service.GetByCategory(category);
            }

            return _service.GetByType(type);
        }

        [HttpPost]
        public ActionResult<Question> CreateAsync([FromForm] QuestionViewModel question)
        {
            Question q = new Question
            {
                Category = question.Category,
                Content = question.Content,
                QuestionHeading = question.QuestionHeading,
                Score = question.Score,
                Type = question.Type
            };

            byte[] contents = null;
            if (question.ContentImage != null || question.ContentImage.Length > 0)
            {
                contents = new byte[question.ContentImage.Length];
                using (var stream = question.ContentImage.OpenReadStream())
                {
                    for (int i = 0; i < question.ContentImage.Length; i++)
                    {
                        contents[i] = (byte)stream.ReadByte();
                    }

                    q.ContentImage = contents;
                }
            }
            
            _service.Create(q);

            return Ok(new { status = true, questionId = q.Id, message = "Question created successfully" }); //CreatedAtRoute("GetQuestion", new { id = question.Id }, question);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Question questionIn)
        {
            var result = _service.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            _service.Update(id, questionIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var question = _service.Get(id);
            if (question == null)
            {
                return NotFound();
            }
            _service.Remove(question.Id);
            return NoContent();
        }
    }
}