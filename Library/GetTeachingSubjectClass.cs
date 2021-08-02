using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class GetTeachingSubjectClass
    {
        public string teahcerName { get; set; }
        public string teacherId { get; set; }
        public string classId { get; set; }
        public List<TeachingSubject> data { get; set; }
    }
    public class TeachingSubject
    {
        public string id { get; set; }
        public string grade_subject_id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string description { get; set; }
        public string _short { get; set; }
        public string max_score { get; set; }
        public string score_type { get; set; }
        public string complementary_score { get; set; }
    }
}
