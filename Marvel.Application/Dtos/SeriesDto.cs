using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Application.Dtos
{
    public class SeriesDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
}
