using SwaggerGlobalization.Models.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Models
{
    public class BaseResponse
    {
        public string RequestStatus { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ErrorMessage
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
    public enum RequestStatus
    {
        KO = 0,
        OK = 1
    }

    public enum AllOrAtLeastOneRequiredType
    {
        AtLeastOneRequired = 0,
        All = 1,
        AllIfOneIsNotNull = 2

    }

    public enum FakeEnum
    {
        //example of display name if you want get localized description using extension methods. Use enumname_propertyname as resource convention
        //with _enumsManager.GetDisplayValue enum extension mehod you can get localized description; with _enumsManager.ToList<FakeEnum> you can convert enum into a KeyValue list with elements made by Key=numeric id, Value=localized description
        [Display(Name = "FakeEnum_FirstFake")]
        FirstFake = 100,
        [Display(Name = "FakeEnum_SecondFake")]
        SecondFake = 200
    }

    public class TestResponse : BaseResponse
    {
        public TestDto Test { get; set; }

    }

    public class TestDto
    {
       public Guid Rand { get; set; }

    }

    public class TestRequest
    {
        [Required(ErrorMessage = "required_field")] //is used resources localization
        public string Fake { get; set; }

       
        [ValidEnum(ErrorMessage = "validenum_field")] //validate input enum
        public FakeEnum FakeEnum { get; set; }

    }
    public class KeyValueDto
    {
        public dynamic Key { get; set; }
        public dynamic Value { get; set; }

        public KeyValueDto()
        {
        }

        public KeyValueDto(dynamic key, dynamic value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key.ToString() + ":" + (Value == null ? string.Empty : Value.ToString().Trim());
            return ret;
        }
    }

    public class KeyValueStringDto
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValueStringDto()
        {
        }

        public KeyValueStringDto(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key + ":" + (Value == null ? string.Empty : Value.Trim());
            return ret;
        }
    }

    public class KeyValueIntDto
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public KeyValueIntDto()
        {
        }

        public KeyValueIntDto(int key, string value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key.ToString() + ":" + (Value == null ? string.Empty : Value.Trim());
            return ret;
        }
    }

}
