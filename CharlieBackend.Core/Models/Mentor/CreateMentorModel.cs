﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Mentor
{
    public class CreateMentorModel : MentorModel
    {
        [JsonIgnore]
        public override long Id { get; set; }

        [Required]
        public override string Email { get; set; }

        [Required]
        [JsonPropertyName("first_name")]
        public override string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public override string LastName { get; set; }

        [Required]
        [JsonPropertyName("course_ids")]
        public new List<long> CourseIds { get; set; }
    }
}
