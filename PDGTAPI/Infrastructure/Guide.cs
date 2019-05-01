using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class Guide
    {
        public Guide()
        {
            Exercise = new HashSet<Exercise>();
        }

        public int Id { get; set; }
        public byte[] GuideImage { get; set; }
        public string GuideDescription { get; set; }

        public ICollection<Exercise> Exercise { get; set; }
    }
}
