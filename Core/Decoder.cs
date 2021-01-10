using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Decoder
    {
        public string Decode(string encoded)
        {
            return Obfusc(encoded);
        }

        // local function obfusc50(s)
        //    local err, t = sep(s)
        //    for i = 1, 5 do
        //       t[i], t[51-i] = t[51-i], t[i]
        //    end

        //    for i = 11, 24 do
        //       t[i], t[51-i] = t[51-i], t[i]
        //    end

        //    return table.concat(t)
        // end
        private string Obfusc50(string s)
        {
            // Swap [0:1:4] with [49:-1:45]
            // Swap [10:1:23] with [39:-1:26]
            var a = s.Substring(0, 5).Reverse();
            var b = s.Substring(5, 5).ToCharArray();
            var c = s.Substring(10, 14).Reverse();
            var d = s.Substring(24, 2).ToCharArray();
            var e = s.Substring(26, 14).Reverse();
            var f = s.Substring(40, 5).ToCharArray();
            var g = s.Substring(45, 5).Reverse();

            var result = g
                .Concat(b)
                .Concat(e)
                .Concat(d)
                .Concat(c)
                .Concat(f)
                .Concat(a);
            
            return string.Concat(result);
        }

        // local function obfusc(s)
        //    local r = ''

        //    while s:len() > 50 do
        //       p = s:sub(1, 50)
        //       s = s:sub(51, -1)
        //       if s:len() < 2 then
        //          r = r .. p
        //       else
        //          r = r .. obfusc50(p)
        //       end
        //    end
        //    r = r .. s
        //    return r
        // end
        private string Obfusc(string s)
        {
            var r = "";
            while (s.Length > 50)
            {
                var p = s.Substring(0, 50);
                s = s.Substring(50);
                if (s.Length < 2)
                {
                    r = $"{r}{p}";
                }
                else
                {
                    r = $"{r}{Obfusc50(p)}";
                }
            }
            r = $"{r}{s}";
            return r;
        }
    }
}
