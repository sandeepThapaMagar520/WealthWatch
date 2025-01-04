using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthWatch.Models
{
    public class Tags
    {
        public Guid TagId { get; set; } = Guid.NewGuid();
        public string TagName { get; set; } = string.Empty;
    }
}
