namespace Homeapp.Backend.Managers
{
    using System;

    /// <summary>
    /// The data manager base class.
    /// </summary>
    public class HomeappDataManagerBase
    {
        /// <summary>
        /// Converts an int array into a DateTime object.
        /// </summary>
        /// <param name="dateArray">The int array.</param>
        /// <returns>The int array must be in m/d/yyyy format.</returns>
        protected DateTime GetDateFromIntArray(int[] dateArray)
        {
            return new DateTime(dateArray[2], dateArray[0], dateArray[1]);
        }
    }
}
