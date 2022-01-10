namespace Homeapp.Backend.Entities.Common.Requests
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Object containing properties to create a new address record in the database.
    /// </summary>
    public class AddressRequest
    {
        /// <summary>
        /// (Optional, provide if editing an existing address) The unique id of the address.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// (Optional) The name of the business
        /// </summary>
        [JsonProperty]
        public string BusinessName { get; set; }

        /// <summary>
        /// The street of the address.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'StreetAddress' is required.")]
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city of the address.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'City' is required.")]
        public string City { get; set; }

        /// <summary>
        /// The state that the address is located in.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'State' is required.")]
        public string State { get; set; }

        /// <summary>
        /// The country that the address is located in.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'Country' is required.")]
        public string Country { get; set; }

        /// <summary>
        /// The zip code.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'ZipCode' is required.")]
        public int ZipCode { get; set; }

        /// <summary>
        /// Bool indicating whether or not the address is an international one.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'InternationalAddress' is required.")]
        public bool InternationalAddress { get; set; }
    }
}
