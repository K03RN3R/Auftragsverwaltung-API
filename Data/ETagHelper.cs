using System;
using System.Security.Cryptography;
using System.Text;


namespace GUI_2.Data
{
    public static class ETagHelper
    {
        //Wandelt RowVersion(byte[]) in ETag-String um
        public static string ToEtag(byte[] rowversion)
        {
            return "\"" + Convert.ToBase64String(rowversion) + "\"";
        }

        //If-Match Vergleich
        public static bool Matches(string? etagHeader, byte[] rowversion)
        {
            if (etagHeader == null)
                return false;

            var currentEtag = ToEtag(rowversion);
            return string.Equals(currentEtag, etagHeader, StringComparison.Ordinal);
        }
    }
}
