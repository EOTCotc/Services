using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Models.Response
{
    public class ComSelectRespon
    {
        public Area country
        {
            get; set;
        }
        public Area province
        {
            get; set;
        }
        public Area city
        {
            get; set;
        }
        public Area county
        {
            get; set;
        }
    }
}
