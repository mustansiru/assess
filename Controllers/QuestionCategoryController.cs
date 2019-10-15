using System;
using System.Collections.Generic;
using assess.Models;
using assess.Services;
using Microsoft.AspNetCore.Mvc;

namespace assess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionCategoryController : ControllerBase
    {
        private readonly QuestionCategoryService _service;

        public QuestionCategoryController(QuestionCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<QuestionCategory>> Get() =>
            _service.Get();

        [HttpGet("{id:length(24)}", Name ="GetCategory")]
        public ActionResult<QuestionCategory> Get(string id)
        {
            var category = _service.Get(id);
            return category == null ? NotFound() : (ActionResult<QuestionCategory>)category;
        }

        [HttpPost]
        public ActionResult<QuestionCategory> Create(QuestionCategory category)
        {
            _service.Create(category);
            return CreatedAtRoute("GetCategory", new { id = category.Id.ToString() }, category);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, QuestionCategory categoryIn)
        {
            var result = _service.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            _service.Update(id, categoryIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var category = _service.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            _service.Remove(category.Id);
            return NoContent();
        }
    }
}
