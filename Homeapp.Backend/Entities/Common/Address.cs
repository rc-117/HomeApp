namespace Homeapp.Backend.Entities
{
    using System;

    /// <summary>
    /// The address class.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The unique id of the address.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// (Optional) The name of the business
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// The street of the address.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city of the address.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state that the address is located in.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The country that the address is located in.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The zip code.
        /// </summary>
        public int ZipCode { get; set; }

        /// <summary>
        /// Bool indicating whether or not the address is an international one.
        /// </summary>
        public bool InternationalAddress { get; set; }
    }
}
