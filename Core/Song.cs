using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Song
    {
        // https://irealpro.com/ireal-pro-file-format/
        // After the irealbook:// URL scheme identifier, we have six components separated by the ‘=’ character (for this reason the ‘=’ cannot be used in the staff text within the chord progression)
        // 1 – Song Title (If starting with ‘The’ change the title to ‘Song Title, The’ for sorting purposes)
        // 2 – Composer’s LastName FirstName (we put the last name first for sorting purposes within the app)
        // 3 – Style (A short text description of the style used for sorting in the app. Medium Swing, Ballad, Pop, Rock…)
        // 4 – Key Signature (C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B, A-, Bb-, B-, C-, C#-, D-, Eb-, E-, F-, F#-, G-, G#-)
        // 5 – n (no longer used)
        //  6 – Chord Progression (This is the main part of this url and will be explained in detail below)
        public string SongTitle { get; set; }
        public string Composer { get; set; }
        public string Style { get; set; }
        public KeySignature KeySignature { get; set; }
        public SongChart SongChart { get; set; }
    }
}
