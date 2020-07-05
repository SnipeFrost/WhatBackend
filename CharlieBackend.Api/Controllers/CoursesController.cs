﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Course;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _coursesService;

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpPost]
        public async Task<ActionResult> PostCourse(CreateCourseModel courseModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isCourseNameTaken = await _coursesService.IsCourseNameTakenAsync(courseModel.Name);
            if (isCourseNameTaken) return StatusCode(409, "Course already exists!");

            var createdCourse = await _coursesService.CreateCourseAsync(courseModel);
            if (createdCourse == null) return StatusCode(500);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseModel>>> GetAllCourses()
        {
            try
            {
                var courses = await _coursesService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch { return StatusCode(500); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCourse(long id, UpdateCourseModel courseModel)
        {
            //if (id != courseModel.Id) return BadRequest();

            try
            {
                var updatedCourse = await _coursesService.UpdateCourseAsync(courseModel);
                if (updatedCourse != null) return NoContent();
                else return StatusCode(409, "Course already exists!");

            }
            catch { return StatusCode(500); }
        }
    }
}
