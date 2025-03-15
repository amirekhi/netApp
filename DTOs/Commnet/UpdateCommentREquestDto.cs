using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.DTOs.Commnet
{
    public class UpdateCommentREquestDto
    {   
        
        [Required]
        [MinLength(5 , ErrorMessage = "Title must be over 5 characters")]
        [MaxLength(280 , ErrorMessage = "Title must be under 280 characters")]
        public string Title {get; set;} = string.Empty;

        [Required]
        [MinLength(5 , ErrorMessage = "Title must be over 5 content")]
        [MaxLength(280 , ErrorMessage = "Title must be under 280 content")]
        public string Content {get; set;} = string.Empty;
    }
}