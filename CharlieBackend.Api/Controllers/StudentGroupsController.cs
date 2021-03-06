﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.StudentGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/student_groups")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {
        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupsController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteStudentGroup(long id)
        //{
        //    var x = await _studentGroupService.SearchStudentGroup(id);
        //    if (x == null)
        //        return Ok("Not Found");
        //    else
        //    {
        //        _studentGroupService.DeleteStudentGrop(id);
        //        return Ok("Done");
        //    }
        //}

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<ActionResult> PostStudentGroupController(CreateStudentGroupModel studentGroup)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isStudentGroupNameChangable = await _studentGroupService.IsGroupNameTakenAsync(studentGroup.Name);
            if (!isStudentGroupNameChangable) return StatusCode(409, "Student Group already exists!");

            var createdStudentGrouprModel = await _studentGroupService.CreateStudentGroupAsync(studentGroup);
            if (createdStudentGrouprModel == null) return StatusCode(422, "Cannot create student group.");

            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudentGroup(long id, UpdateStudentGroupModel studentGroup)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var isStudentGroupNameChangable = await _studentGroupService.IsGroupNameTakenAsync(studentGroup.Name);
                if (!isStudentGroupNameChangable) return StatusCode(409, "Student group name is already taken!");

                studentGroup.Id = id;
                var updatedStudentGroup = await _studentGroupService.UpdateStudentGroupAsync(studentGroup);
                if (updatedStudentGroup != null) return NoContent();
                else return StatusCode(409, "Cannot update.");
            }
            catch { return StatusCode(500); }
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<List<StudentGroupModel>>> GetAllStudentGroups()
        {
            try
            {
                var studentGroupsModels = await _studentGroupService.GetAllStudentGroupsAsync();
                return Ok(studentGroupsModels);
            }
            catch { return StatusCode(500); }
        }

        [Authorize(Roles ="2")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroupById>> GetStudentGroupById(long id)
        {
            var foundStudentGroup = await _studentGroupService.GetStudentGroupByIdAsync(id);
            if (foundStudentGroup == null) return BadRequest("No such student group.");

            return foundStudentGroup;
        }
    }
}
