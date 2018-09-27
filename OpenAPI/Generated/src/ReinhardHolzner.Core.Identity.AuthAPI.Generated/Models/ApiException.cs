/*
 * RHCore Identity Auth API
 *
 * The RHCore Identity Auth API provides the most common methods to handle authentication server side using ASP.NET Identity Core.
 *
 * OpenAPI spec version: 1.0.0-s2
 * Contact: holzner@invest-fit.at
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ReinhardHolzner.Core.Identity.AuthAPI.Generated.Models
{ 
    /// <summary>
    /// If the API call returns HTTP status codes indicating errors, the response body will contain an *ApiException* object providing more information about the error that occured
    /// </summary>
    [DataContract]
    [NotMapped]
	public partial class ApiException : IEquatable<ApiException>
    { 
		private string _ErrorCode;
		
		/// <summary>
        /// The error code
        /// </summary>
        /// <value>The error code</value>
        [DataMember(Name="error_code")]
		public string ErrorCode { get => _ErrorCode; set { _ErrorCode = value; ErrorCodeSet = true; } }
		
		public bool ErrorCodeSet = false;		

		private string _ErrorMessage;
		
		/// <summary>
        /// The error message
        /// </summary>
        /// <value>The error message</value>
        [DataMember(Name="error_message")]
		public string ErrorMessage { get => _ErrorMessage; set { _ErrorMessage = value; ErrorMessageSet = true; } }
		
		public bool ErrorMessageSet = false;		

		private string _ErrorDetails;
		
		/// <summary>
        /// More details about the error, if available
        /// </summary>
        /// <value>More details about the error, if available</value>
        [DataMember(Name="error_details")]
		public string ErrorDetails { get => _ErrorDetails; set { _ErrorDetails = value; ErrorDetailsSet = true; } }
		
		public bool ErrorDetailsSet = false;		

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiException {\n");
            sb.Append("  ErrorCode: ").Append(ErrorCode).Append("\n");
            sb.Append("  ErrorMessage: ").Append(ErrorMessage).Append("\n");
            sb.Append("  ErrorDetails: ").Append(ErrorDetails).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ApiException)obj);
        }

        /// <summary>
        /// Returns true if ApiException instances are equal
        /// </summary>
        /// <param name="other">Instance of ApiException to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiException other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ErrorCode == other.ErrorCode ||
                    ErrorCode != null &&
                    ErrorCode.Equals(other.ErrorCode)
                ) && 
                (
                    ErrorMessage == other.ErrorMessage ||
                    ErrorMessage != null &&
                    ErrorMessage.Equals(other.ErrorMessage)
                ) && 
                (
                    ErrorDetails == other.ErrorDetails ||
                    ErrorDetails != null &&
                    ErrorDetails.Equals(other.ErrorDetails)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (ErrorCode != null)
                    hashCode = hashCode * 59 + ErrorCode.GetHashCode();
                    if (ErrorMessage != null)
                    hashCode = hashCode * 59 + ErrorMessage.GetHashCode();
                    if (ErrorDetails != null)
                    hashCode = hashCode * 59 + ErrorDetails.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ApiException left, ApiException right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiException left, ApiException right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
