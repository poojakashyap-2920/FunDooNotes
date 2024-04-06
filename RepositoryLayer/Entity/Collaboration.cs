using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
   public class Collaboration
    {
        [Key]
        public int collaborationid { get; set; }

        [ForeignKey("UserEntity")]
        public string useremail { get; set; }

        [ForeignKey("NotesEntity")]
        public int collabnoteid { get; set; }
        public string? emailid { get; set; }
    }
}
