﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace HCore.Web.Exceptions
{
    public class RequestFailedApiException : ApiException
    {
        private readonly string _errorCode;

        public const string ArgumentInvalid = "argument_invalid";
        public const string ArgumentMissing = "argument_missing";
        public const string MaxPagingOffsetExceeded = "max_paging_offset_exceeded";
        public const string MaxPagingLimitExceeded = "max_paging_limit_exceeded";
        public const string PagingOffsetInvalid = "paging_offset_invalid";
        public const string PagingLimitInvalid = "paging_limit_invalid";
        public const string ApiCredentialsMissing = "api_credentials_missing";
        public const string ClientIdMissing = "client_id_missing";
        public const string ClientIdInvalid = "client_id_invalid";
        public const string ClientIdTooLong = "client_id_too_long";
        public const string ClientSecretMissing = "client_secret_missing";
        public const string ClientSecretInvalid = "client_secret_invalid";
        public const string ClientSecretTooLong = "client_secret_too_long";
        public const string RedirectUrlNotRequired = "redirect_url_not_required";
        public const string RedirectUrlMissing = "redirect_url_missing";
        public const string RedirectUrlInvalid = "redirect_url_invalid";
        public const string RedirectUrlTooLong = "redirect_url_too_long";
        public const string RedirectUrlMustBeAbsolute = "redirect_url_must_be_absolute";
        public const string UuidMissing = "uuid_missing";
        public const string UuidInvalid = "uuid_invalid";
        public const string StateInvalid = "state_invalid";
        public const string CodeMissing = "code_missing";
        public const string CodeInvalid = "code_invalid";
        public const string CodeTooLong = "code_too_long";
        public const string IssuerMissing = "issuer_missing";
        public const string IssuerInvalid = "issuer_invalid";
        public const string IssuerTooLong = "issuer_too_long";
        public const string SubjectMissing = "subject_missing";
        public const string SubjectInvalid = "subject_invalid";
        public const string SubjectTooLong = "subject_too_long";
        public const string PrivateKeyMissing = "private_key_missing";
        public const string PrivateKeyNoDelimiters = "private_key_no_delimiters";
        public const string PrivateKeyNoPrivateKey = "private_key_no_private_key";
        public const string PrivateKeyInvalid = "private_key_invalid";
        public const string PrivateKeyTooLong = "private_key_too_long";
        public const string UserCredentialsMissing = "user_credentials_missing";
        public const string UserNameMissing = "user_name_missing";
        public const string UserNameInvalid = "user_name_invalid";
        public const string UserNameTooLong = "user_name_too_long";
        public const string PasswordMissing = "password_missing";        
        public const string PasswordTooShort = "password_too_short";
        public const string PasswordTooLong = "password_too_long";
        public const string PasswordConfirmationMissing = "password_confirmation_missing";
        public const string PasswordConfirmationNoMatch = "password_confirmation_no_match";
        public const string NameMissing= "name_missing";
        public const string NameInvalid = "name_invalid";
        public const string NameTooLong = "name_too_long";
        public const string TooManyUpdateRecords = "too_many_update_records";
        public const string NoRecordsSpecified = "no_records_specified";
        public const string UserGroupUuidMissing = "user_group_uuid_missing";
        public const string UserGroupUuidInvalid = "user_group_uuid_invalid";
        public const string UserUuidMissing = "user_uuid_missing";
        public const string UserUuidInvalid = "user_uuid_invalid";
        public const string UserUuidTooLong = "user_uuid_too_long";
        public const string EmailMissing = "email_missing";
        public const string EmailInvalid = "email_invalid";
        public const string EmailTooLong = "email_too_long";
        public const string EmailAlreadyExists = "email_already_exists";
        public const string PhoneNumberMissing = "phone_number_missing";
        public const string PhoneNumberInvalid = "phone_number_invalid";
        public const string PhoneNumberTooLong = "phone_number_too_long";
        public const string ValidationFailed = "validation_failed";
        public const string DuplicateUserName = "duplicate_user_name";
        public const string PasswordRequiresNonAlphanumeric = "password_requires_non_alphanumeric";
        public const string SecurityTokenInvalid = "security_token_invalid";
        public const string ScrollUuidInvalid = "scroll_uuid_invalid";
        public const string ScrollUuidTooLong = "scroll_uuid_too_long";
        public const string FirstNameMissing = "first_name_missing";
        public const string FirstNameInvalid = "first_name_invalid";
        public const string FirstNameTooLong = "first_name_too_long";
        public const string LastNameMissing = "last_name_missing";
        public const string LastNameInvalid = "last_name_invalid";
        public const string LastNameTooLong = "last_name_too_long";
        public const string UserDetailsNotSupported = "user_details_not_supported";
        public const string CannotDeleteLastSystemUserGroupMember = "cannot_delete_last_system_user_group_member";
        public const string SortByInvalid = "sort_by_invalid";
        public const string SortOrderInvalid = "sort_order_invalid";
        public const string PleaseAcceptPrivacyPolicy = "please_accept_privacy_policy";
        public const string PleaseAcceptTermsAndConditions = "please_accept_terms_and_conditions";
        public const string NotificationCultureInvalid = "notification_culture_invalid";

        public RequestFailedApiException(string errorCode, string message) : 
            base(message)
        {
            _errorCode = errorCode;            
        }

        public RequestFailedApiException(string errorCode, string message, string uuid, string name) :
           base(message, uuid, name)
        {
            _errorCode = errorCode;
        }

        public RequestFailedApiException(string errorCode, string message, long? uuid, string name) :
           base(message, uuid, name)
        {
            _errorCode = errorCode;
        }

        public RequestFailedApiException(string errorCode, string message, DateTimeOffset? dateTimeOffset) :
           base(message, dateTimeOffset)
        {
            _errorCode = errorCode;
        }

        public override int GetStatusCode()
        {
            return StatusCodes.Status400BadRequest;
        }

        public override string GetErrorCode()
        {
            return _errorCode;
        }      
    }
}
