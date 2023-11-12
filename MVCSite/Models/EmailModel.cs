using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjetosViniciusVieiraMota.Models
{
    public class EmailModel
    {
        [Key]
        public string email {  get; set; }
    }
}
