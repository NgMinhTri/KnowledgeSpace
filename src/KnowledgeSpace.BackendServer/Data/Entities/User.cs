using KnowledgeSpace.BackendServer.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("Users")]
    public class User : IdentityUser, IDateTracking
    {
        

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        public int? NumberOfKnowledgeBases { get; set; }

        public int? NumberOfVotes { get; set; }

        public int? NumberOfReports { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get ; set ; }
    }
}