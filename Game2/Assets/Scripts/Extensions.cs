using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public static class Extensions {

        public static bool Contains(this string theString, IEnumerable<string> testStrings){
            return testStrings.Any(s => theString.ToLower().Contains(s.ToLower()));
        }
    }


}