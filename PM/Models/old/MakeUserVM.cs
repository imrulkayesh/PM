using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public class MakeUserVM
    {
        public string USER_TEXT { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PASS { get; set; }
        public string USER_CONT { get; set; }
        public Nullable<byte> USER_LEVL { get; set; }
        public string USER_TYPE { get; set; }
        public string USER_DEPT { get; set; }
        public string USER_COMP { get; set; }
        public string USER_ADMN { get; set; }
        public Nullable<bool> USER_ACTV { get; set; }
        public string USER_IMAG { get; set; }
    }
}