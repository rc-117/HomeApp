namespace Homeapp.Backend.Tools
{
    using System;

    /// <summary>
    /// Class to get a reason phrase from an enum.
    /// </summary>
    public static class HttpReasonPhrase
    {
        /// <summary>
        /// Gets a phrase string from the ReasonPhrase enum.
        /// </summary>
        /// <param name="phrase"></param>
        public static string GetPhrase(ReasonPhrase phrase)
        {
            return Enum.GetName(typeof(ReasonPhrase), phrase);
        }
    }
}
